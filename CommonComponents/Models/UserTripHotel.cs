using System;

namespace CommonComponents.Models
{
    public class UserTripHotel
    {
        public Guid ReservationId { get; set; }
        public short HotelId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FoodType { get; set; }
    }
}