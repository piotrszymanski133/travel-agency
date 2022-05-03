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
        public HotelWithDescription GetHotelWithDescription(string hotelId);
        public HotelStateOnDay findFreeRoomsForReservationTime(Hotel hotel, DateTime start, DateTime end);
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
            List<Hotel> hotels = new List<Hotel>();
            using var db = new hotelsContext();
            if (tripParameters.Destination == "any")
            {
                hotels = db.Hotels
                    .Include(h => h.Events)
                    .ThenInclude(e => e.Eventrooms)
                    .Include(h => h.Destination)
                    .Include(h => h.Hotelrooms)
                    .ThenInclude(r => r.Roomtype)
                    .ToList();
            }
            else
            {
                hotels = db.Hotels
                    .Include(h => h.Events)
                    .ThenInclude(e => e.Eventrooms)
                    .Include(h => h.Destination)
                    .Include(h => h.Hotelrooms)
                    .ThenInclude(r => r.Roomtype)
                    .Where(h => h.Destination.Country == tripParameters.Destination)
                    .ToList();
            }

            List<CommonComponents.Models.Hotel> offeredHotels = new List<CommonComponents.Models.Hotel>();
            foreach (Hotel hotel in hotels)
            {
                bool onlyPremiumRoomsAvailable = true;
                int neededCapacity = tripParameters.Adults + tripParameters.ChildrenUnder3 +
                                     tripParameters.ChildrenUnder10 +
                                     tripParameters.ChildrenUnder18;
                HotelStateOnDay hotelStateOnDay =
                    findFreeRoomsForReservationTime(hotel, tripParameters.StartDate, tripParameters.EndDate);
                int hotelCapacity = 0;
                List<HotelRoom> suitableRooms = hotelStateOnDay.FreeRooms.FindAll(room =>
                    room.CapacityPeople == neededCapacity && room.Quantity > 0);
                if (suitableRooms.Count > 0)
                {
                    suitableRooms.ForEach(room =>
                    {
                        if (!room.Name.EndsWith("Premium"))
                        {
                            onlyPremiumRoomsAvailable = false;
                        }
                    });
                    offeredHotels.Add(new CommonComponents.Models.Hotel()
                    {
                        Id = hotel.Id,
                        DestinationCity = hotel.Destination.City,
                        DestinationCountry = hotel.Destination.Country,
                        Food = hotel.Food,
                        Name = hotel.Name,
                        Rating = hotel.Rating,
                        Stars = hotel.Stars.GetValueOrDefault(),
                        IsOnlyPremiumAvailable = onlyPremiumRoomsAvailable
                    });
                }
            }
            return offeredHotels;
            
        }

        public HotelStateOnDay findFreeRoomsForReservationTime(Hotel hotel, DateTime start, DateTime end)
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
            List<Hotel> hotels = db.Hotels
                .Include(h => h.Destination)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Roomtype)
                .ToList();
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

        public HotelWithDescription GetHotelWithDescription(string hotelId)
        {
            using var db = new hotelsContext();
            db.Hotels
                .Include(h => h.Destination)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Roomtype)
                .ToList();
            Hotel hotel = db.Hotels.Find(hotelId);
            HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).FirstOrDefault();
            return new HotelWithDescription(hotel, desc);

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