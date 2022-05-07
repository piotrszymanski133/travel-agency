using System;
using System.Collections.Generic;
using System.Linq;
using ApiGateway.Models;
using CommonComponents.Exceptions;
using CommonComponents.Models;
using HotelsService.Models;
using HotelsService.Services;
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

        public void CreateReservationEvent(Hotel hotel, Guid tripReservationId, int roomTypeId, DateTime start,
            DateTime end, string username);

        public List<CommonComponents.Models.Hotel> GetHotels(TripParameters tripParameters);
        public HotelWithDescription GetHotelWithDescription(short hotelId);
        public HotelStateOnDay findFreeRoomsForReservationTime(Hotel hotel, DateTime start, DateTime end);
        void rollbackReservation(Guid messageTripReservationId);
        void confirmOrder(Guid messageReservationId);
        List<Event> GetUserOrders(string messageUsername);
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
            _descriptions =
                _mongoDatabase.GetCollection<HotelDescription>(hotelDescriptionDbSettings.Value.CollectionName);
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
                    .Include(h => h.Destination)
                    .Include(h => h.Hotelrooms)
                    .ThenInclude(r => r.Roomtype)
                    .ToList();
            }
            else
            {
                hotels = db.Hotels
                    .Include(h => h.Events)
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
                    CommonComponents.Models.Hotel offered = new CommonComponents.Models.Hotel()
                    {
                        Id = hotel.Id,
                        DestinationCity = hotel.Destination.City,
                        DestinationCountry = hotel.Destination.Country,
                        Food = hotel.Food,
                        Name = hotel.Name,
                        Rating = hotel.Rating,
                        Stars = hotel.Stars.GetValueOrDefault(),
                        IsOnlyPremiumAvailable = onlyPremiumRoomsAvailable
                    };
                    offered.LowestPrice =
                        PriceCalculator.CalculateHotelLowestPrice(offered, tripParameters, onlyPremiumRoomsAvailable);
                    offeredHotels.Add(offered);
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

            short[] maxReserved = new short[12];
            for (int i = 0; i < 12; i++)
                maxReserved[i] = 0;

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                var reservations = hotel.Events.Where(e => e.StartDate <= date && e.EndDate >= date)
                    .GroupBy(e => e.RoomTypeId)
                    .Select(e => new KeyValuePair<short, int>(e.Key, e.ToList().Count));

                var enumerator = reservations.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (maxReserved[enumerator.Current.Key] < enumerator.Current.Value)
                        maxReserved[enumerator.Current.Key] = (short)enumerator.Current.Value;
                }
            }

            for (int i = 0; i < maxHotelState.FreeRooms.Count; i++)
            {
                maxHotelState.FreeRooms[i].Quantity -= maxReserved[maxHotelState.FreeRooms[i].RoomtypeId];
            }

            return maxHotelState;
        }

        public void rollbackReservation(Guid tripReservationId)
        {
            using var db = new hotelsContext();
            Event e = db.Events.First(e => e.TripReservationId == tripReservationId);
            db.Remove(e);
            db.SaveChanges();
        }

        public void confirmOrder(Guid tripReservationId)
        {
            using var db = new hotelsContext();
            Event e = db.Events.First(e => e.TripReservationId == tripReservationId);
            e.Type = "Ordered";
            db.SaveChanges();
        }

        public List<Event> GetUserOrders(string messageUsername)
        {
            using var db = new hotelsContext();
            return db.Events
                .Include(e => e.Hotel)
                .ThenInclude(h => h.Destination)
                .Where(e => e.Username == messageUsername && e.Type == "Order")
                .ToList();
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
                List<Event> events = db.Events.Where(e => e.HotelId == hotel.Id).ToList();
                HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).First();
                hotelsWithDescriptions.Add(new HotelWithDescription(hotel, desc));
            }

            return hotelsWithDescriptions;
        }

        public HotelWithDescription GetHotelWithDescription(short hotelId)
        {
            using var db = new hotelsContext();
            db.Hotels
                .Include(h => h.Destination)
                .Include(h => h.Hotelrooms)
                .ThenInclude(r => r.Roomtype)
                .Include(h => h.Events)
                .ToList();
            Hotel hotel = db.Hotels.Find(hotelId);
            if (hotel == null)
            {
                throw new HotelDoesNotExistException($"Hotel with id {hotelId} does not exist!");
            }

            HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).FirstOrDefault();
            return new HotelWithDescription(hotel, desc);
        }

        public void CreateReservationEvent(Hotel hotel, Guid tripReservationId, int roomTypeId, DateTime start,
            DateTime end, string username)
        {
            using (var db = new hotelsContext())
            {
                Event e = new Event()
                {
                    Username = username,
                    TripReservationId = tripReservationId,
                    StartDate = start.AddHours(4).ToUniversalTime().Date,
                    EndDate = end.AddHours(4).ToUniversalTime().Date,
                    HotelId = hotel.Id,
                    Type = "Reservation",
                    RoomTypeId = (short) roomTypeId,
                    Id = Guid.NewGuid()
                };
                db.Events.Add(e);
                db.SaveChanges();
            }
        }
    }
}