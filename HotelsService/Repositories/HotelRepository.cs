using System;
using System.Collections.Generic;
using System.Linq;
using CommonComponents.Models;
using HotelsService.Models;
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
        public void CreateReservationEvent();
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

        public void CreateReservationEvent()
        {
            using (var db = new hotelsContext())
            {
                Hotel h = GetAllHotels().First();
                Event e = new Event()
                {
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