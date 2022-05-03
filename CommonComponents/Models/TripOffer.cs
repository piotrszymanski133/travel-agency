using System;
using System.Collections.Generic;

namespace CommonComponents.Models
{
    public class TripOffer
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public HotelOffer HotelOffer { get; set; }
        public List<TransportOffer> TransportOffers { get; set; }
    }
}