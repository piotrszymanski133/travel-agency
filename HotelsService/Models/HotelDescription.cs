using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelsService.Models
{
    public class HotelDescription
    {
        [BsonId]
        public string Id { get; set; } = null!;
        
        [BsonElement("Description")]
        public string Description { get; set; } = null!;

        public HotelDescription(string id, string description)
        {
            Id = id;
            Description = description;
        }
        
        public HotelDescription()
        {
        }

    }
}