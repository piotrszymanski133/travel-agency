using System;
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

    public class GetHotelOfferResponse
    {
        public HotelOffer HotelOffer { get; set; }
    }

    public class GetTransportOffersResponse
    {
        public List<TransportOffer> TransportOffer { get; set; }
    }

    public class ReserveTripResponse
    {
        public int Price { get; set; }
        public bool Success { get; set; }
        public Guid ReservationId { get; set; }
    }
    
    public class ReserveHotelSuccessResponse
    {
        public Guid ReservationId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HotelName { get; set; }
        public string FoodType { get; set; }
        public string ReservedRoomName { get; set; }
        public int Price { get; set; }
    }

    public class ReserveHotelFailureResponse
    {
        public Guid ReservationId { get; set; }
    }

    public class ReserveTransportSuccessResponse
    {
        public Guid ReservationId { get; set; }
        public int Price { get; set; }
        public string TransportTypeName { get; set; }
    }

    public class ReserveTransportFailureResponse
    {
        public Guid ReservationId { get; set; }
    }

    public class PaymentExpired
    {
        public Guid ReservationId { get; set; }
    }

    public class PaymentResponse
    {
        public Guid ReservationId { get; set; }
        public bool Success { get; set; }
    }

    public class PaymentForTripAcceptedResponse
    {
        public Guid ReservationId { get; set; }
    }

    public class PaymentForTripRejectedResponse
    {
        public Guid ReservationId { get; set; }
    }

    public class GetDestinationsResponse
    {
        public List<string> Destinations { get; set; }
    }

    public class GetUserTripsHotelsResponse
    {
        public List<UserTripHotel> OrderedHotels { get; set; }
    }
    
    public class GetUserTripsTransporResponse
    {
        public string Username { get; set; }
        public List<UserTransports> UserTransportsList { get; set; }
    }
    public class CreateUserTripQueryResponse
    {
        public Guid id { get; set; }
    }
}