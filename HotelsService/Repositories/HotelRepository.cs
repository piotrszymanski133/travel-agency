using System.Collections.Generic;
using System.Linq;
using HotelsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HotelsService.Repositories
{
    public interface IHotelRepository
    {
        public List<HotelWithDescription> GetAllHotels();
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
            db.Hotels.Include(h => h.Destination).ToList();
            db.Hotels.Include(h => h.Hotelrooms).ToList();
            List<Hotel> hotels = db.Hotels.ToList();
            foreach (var hotel in hotels)
            {
                HotelDescription desc = _descriptions.Find(description => description.Id == hotel.Id).First();
                hotelsWithDescriptions.Add(new HotelWithDescription(hotel, desc));
            }

            return hotelsWithDescriptions;
        }
    }
}