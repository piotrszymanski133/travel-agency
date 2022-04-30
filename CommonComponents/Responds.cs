using System.Collections.Generic;
using CommonComponents.Models;

namespace CommonComponents
{
    public class GetTripsRespond
    {
        public List<Trip> Trips { get; set; }
    }


    public class GetHotelsRespond
    {
        public List<Hotel> Hotels { get; set; }
    }
}