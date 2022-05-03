using System.Collections.Generic;
using CommonComponents.Models;

namespace CommonComponents
{
    public class GetTripsResponse
    {
        public List<Trip> Trips { get; set; }
    }
    
    public class GetTripOfferResponse
    {
        public TripOffer TripOffer { get; set; }
    }


    public class GetHotelsResponse
    {
        public List<Hotel> Hotels { get; set; }
    }
    
    public class GetTransportResponse
    {
        public List<Transport> Transports { get; set; }
    }
    
    public class GetHotelOfferResponse
    {
        public HotelOffer HotelOffer { get; set; }
    }

    public class GetTransportOffersResponse
    {
    
    }
}