using System;

namespace CommonComponents.Models
{
    public class UserTrips
    {
        public Guid id { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FoodType { get; set; }
        public string HotelRoomName { get; set; }
        public string TransportTypeName { get; set; }
        public int Persons { get; set; }
    }
}