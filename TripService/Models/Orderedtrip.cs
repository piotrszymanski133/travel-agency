using System;
using System.Collections.Generic;

namespace TripService
{
    public partial class Orderedtrip
    {
        public Guid TripId { get; set; }
        public string HotelName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string RoomTypeName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Food { get; set; } = null!;
        public string TransportTypeName { get; set; } = null!;
        public int Persons { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
