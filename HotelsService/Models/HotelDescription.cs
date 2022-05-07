using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelsService.Models
{
    public class HotelDescription
    {
        [BsonId] public short Id { get; set; }

        [BsonElement("Description")] public string Description { get; set; } = null!;

        public HotelDescription(short id, string description)
        {
            Id = id;
            Description = description;
        }

        public HotelDescription()
        {
        }
    }
}