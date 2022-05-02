using System;
using System.Collections.Generic;
using System.Linq;
using ApiGateway.Models;
using CommonComponents.Models;
using HotelsService.Models;
using MassTransit.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using Hotel = HotelsService.Models.Hotel;

namespace HotelsService.Repositories
{
    public interface IHotelRepository
    {
        public List<HotelWithDescription> GetAllHotels();
        public void CreateReservationEvent(DateTime start, DateTime end);
        public List<CommonComponents.Models.Hotel> GetHotels(TripParameters tripParameters);
    }
    
    public class HotelRepository : IHotelRepository
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<HotelDescription> _descriptions;

        public HotelRepository(IOptions<HotelDescriptionDbSettings> hotelDescriptionDbSettings)
        {
            _mongoClient = new MongoClient(hotelDescriptionDbSettings.Value.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(hotelDescriptionDbSettings.Value.DatabaseName);
            _descriptions = _mongoDatabase.GetCollection<HotelDescription>(hotelDescriptionDbSettings.Value.CollectionName);
        }

        public List<CommonComponents.Models.Hotel> GetHotels(TripParameters tripParameters)
        {
            List<Hotel> matchedHotels = new List<Hotel>();
            using var db = new hotelsContext();
            List<Hotel> hotels = db.Hotels
                .Include(h => h.Events)
                .ThenInclude(e => e.Eventrooms)
                .Include(h => h.Destination)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Roomtype)
                .Where(h => h.Destination.Country == tripParameters.Destination)
                .ToList();

            
            List<CommonComponents.Models.Hotel> hotelOffers = new List<CommonComponents.Models.Hotel>();
            foreach (Hotel hotel in hotels)
            {
                int neededCapacity = tripParameters.Adults + tripParameters.ChildrenUnder3 + tripParameters.ChildrenUnder10 +
                                     tripParameters.ChildrenUnder18;
                HotelStateOnDay hotelStateOnDay =
                    findFreeRoomsForReservationTime(hotel, tripParameters.StartDate, tripParameters.EndDate);
                int hotelCapacity = 0;
                hotelStateOnDay.FreeRooms.ForEach(room => hotelCapacity += room.Quantity * room.CapacityPeople);

                if (hotelCapacity >= neededCapacity)
                {
                    List<HotelRoom> offerRooms = new List<HotelRoom>();
                    while (neededCapacity > 0)
                    {
                        int maxCapacity = hotelStateOnDay.FreeRooms.Max(room => room.CapacityPeople);
                        if (neededCapacity > maxCapacity)
                        {
                            HotelRoom biggestRoom =
                                hotelStateOnDay.FreeRooms.Find(er => er.CapacityPeople == maxCapacity);
                            HotelRoom selected = biggestRoom.shallowCopy();
                            selected.Quantity = (short) (neededCapacity / maxCapacity);
                            offerRooms.Add(selected);
                            biggestRoom.Quantity--;
                            if (biggestRoom.Quantity == 0)
                                hotelStateOnDay.FreeRooms.Remove(biggestRoom);
                            neededCapacity -= maxCapacity * selected.Quantity;
                        }
                        
                        else if (!hotelStateOnDay.FreeRooms.Find(room => room.CapacityPeople == neededCapacity).Equals(null))
                        {
                            HotelRoom foundRoom =
                                hotelStateOnDay.FreeRooms.Find(room => room.CapacityPeople == neededCapacity);
                            HotelRoom selected = foundRoom.shallowCopy();
                            selected.Quantity = 1;
                            offerRooms.Add(selected);
                            neededCapacity = 0;
                        } 
                        
                        else if (!hotelStateOnDay.FreeRooms.Find(room => neededCapacity / room.CapacityPeople >= 0.6).Equals(null))
                        {
                            HotelRoom foundRoom = hotelStateOnDay.FreeRooms
                                .FindAll(room => neededCapacity / room.CapacityPeople >= 0.6).First();
                            HotelRoom selected = foundRoom.shallowCopy();
                            selected.Quantity = 1;
                            offerRooms.Add(selected);
                            foundRoom.Quantity--;
                            if (foundRoom.Quantity == 0)
                                hotelStateOnDay.FreeRooms.Remove(foundRoom);
                            neededCapacity -= foundRoom.CapacityPeople;
                        }

                    }
                    HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).First();
                    hotelOffers.Add(new CommonComponents.Models.Hotel()
                    {
                        Description = desc.Description,
                        DestinationCity = hotel.Destination.City,
                        DestinationCountry = hotel.Destination.Country,
                        Food = hotel.Food,
                        Id = hotel.Id,
                        Name = hotel.Name,
                        Rating = hotel.Rating,
                        Stars = hotel.Stars.GetValueOrDefault(),
                        Rooms = offerRooms
                    });
                }
            }
            return hotelOffers;
        }

        private HotelStateOnDay findFreeRoomsForReservationTime(Hotel hotel, DateTime start, DateTime end)
        {
            HotelStateOnDay maxHotelState = new HotelStateOnDay();
            hotel.Hotelrooms.ForEach(room => maxHotelState.FreeRooms.Add(new HotelRoom()
            {
                Quantity = room.Quantity,
                CapacityPeople = room.Roomtype.CapacityPeople,
                RoomtypeId = room.RoomtypeId,
                Name = room.Roomtype.Name
            }));

            short[] maxReserved = new short[5];
            for (int i = 0; i < 5; i++)
                maxReserved[i] = 0;
            
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                var reservations = hotel.Events.Where(e => e.StartDate <= date && e.EndDate >= date)
                    .SelectMany(e => e.Eventrooms)
                    .GroupBy(e => e.RoomtypeId)
                    .Select(er => er
                        .ToList()
                        .Sum(er => er.Quantity))
                    .ToArray();

                for (int i = 0; i < reservations.Length; i++)
                {
                    if (maxReserved[i] < reservations[i])
                        maxReserved[i] = (short)reservations[i];
                }
            }
            for (int i = 0; i < maxHotelState.FreeRooms.Count; i++)
            {
                maxHotelState.FreeRooms[i].Quantity -= maxReserved[i];
            }
            return maxHotelState;
        }
        public List<HotelWithDescription> GetAllHotels()
        {
            List<HotelWithDescription> hotelsWithDescriptions = new List<HotelWithDescription>();
            using var db = new hotelsContext();
            List<Hotel> hotels = db.Hotels.Include(h => h.Destination).Include(h => h.Hotelrooms).ThenInclude(r => r.Roomtype).ToList();
            foreach (var hotel in hotels)
            {
                List<Event> events = db.Events.Where(e => e.HotelId == hotel.Id).Include(e => e.Eventrooms).ToList();
                if (events.Count < 10)
                {
                    HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).First();
                    hotelsWithDescriptions.Add(new HotelWithDescription(hotel, desc));
                }
            }

            return hotelsWithDescriptions;
        }

        public void CreateReservationEvent(DateTime start, DateTime end)
        {
            using (var db = new hotelsContext())
            {
                Hotel h = GetAllHotels().First();
                Event e = new Event()
                {
                    StartDate = start.AddHours(4).ToUniversalTime().Date,
                    EndDate = end.AddHours(4).ToUniversalTime().Date,
                    HotelId = h.Id,
                    Type = "Reservation",
                    Id = Guid.NewGuid()
                };
                db.Events.Add(e);
                Eventroom eventroom = new Eventroom()
                {
                    Id = Guid.NewGuid(),
                    Quantity = 2,
                    RoomtypeId = h.Hotelrooms.First().RoomtypeId,
                    Event = e,
                };
                e.Eventrooms = new List<Eventroom>()
                {
                    eventroom
                };

                db.SaveChanges();
            }
        }
    }
}