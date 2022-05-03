using System;

namespace CommonComponents.Models
{
    public class TripOfferQueryParameters
    {
        public string HotelId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Adults { get; set; }
        public int ChildrenUnder3 { get; set; }
        public int ChildrenUnder10 { get; set; }
        public int ChildrenUnder18 { get; set; }
        public string Departure { get; set; }
    }
}