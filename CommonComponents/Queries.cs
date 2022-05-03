using System;
using ApiGateway.Models;
using CommonComponents.Models;

namespace CommonComponents;

public class GetTripsQuery
{
    public TripParameters TripParameters { get; set; }
}

public class GetTripOfferQuery
{
    public TripOfferQueryParameters TripOfferQueryParameters { get; set; }
}

public class GetHotelsQuery
{
    public TripParameters TripParameters { get; set; }
}

public class GetTransportQuery
{
    public string DestinationCity { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
}

public class GetHotelOfferQuery
{
    public TripOfferQueryParameters TripOfferQueryParameters { get; set; }
}

public class GetTransportOffersQuery
{
    public TripOfferQueryParameters TripOfferQueryParameters { get; set; }
}
