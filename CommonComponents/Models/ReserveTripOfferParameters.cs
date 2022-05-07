using System;

namespace CommonComponents.Models
{
    public class ReserveTripOfferParameters
    {
        public string HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomTypeId { get; set; }
        public int TransportFromId { get; set; }
        public int TransportToId { get; set; }
        public string Username { get; set; }
        public int Adults { get; set; }
        public int ChildrenUnder3 { get; set; }
        public int ChildrenUnder10 { get; set; }
        public int ChildrenUnder18 { get; set; }
        
        public string PromoCode { get; set; }
    }
}