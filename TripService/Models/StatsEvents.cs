using System;

namespace TripService.Models
{
    public class PurchaseDirectionEvents
    {
        public string Country { get; set; }
        public DateTime  EventDate { get; set; }
    }
    public class PurchasePreferencesEvents
    {
        public string HotelName { get; set; }
        public string TransportType { get; set; }
        public string NameOfRoom { get; set; }
        public DateTime EventDate { get; set; }
    }
}