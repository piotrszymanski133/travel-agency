using System.Collections.Generic;
using CommonComponents.Models;

namespace CommonComponents
{
    public class GetTripsRespond
    {
        public List<Trip> Trips { get; set; }
    }
    
    public class GetTripRespond
    {
        public Trip Trip { get; set; }
    }


    public class GetHotelsRespond
    {
        public List<Hotel> Hotels { get; set; }
    }
    
    public class GetTransportRespond
    {
        public List<Transport> Transports { get; set; }
    }
}