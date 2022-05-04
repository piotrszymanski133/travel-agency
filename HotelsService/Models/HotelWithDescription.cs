namespace HotelsService.Models
{
    public class HotelWithDescription : Hotel
    {
        public string Description { get; set; }

        public HotelWithDescription(Hotel hotel, HotelDescription hotelDescription)
        {
            Id = hotel.Id;
            Name = hotel.Name;
            DestinationId = hotel.DestinationId;
            Destination = hotel.Destination;
            Rating = hotel.Rating;
            Food = hotel.Food;
            Stars = hotel.Stars;
            Description = hotelDescription.Description;
            Hotelrooms = hotel.Hotelrooms;
            Events = hotel.Events;
        }
    }
}