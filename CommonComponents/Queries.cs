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
    public string Departue { get; set; }
    public string Destination { get; set; }
    public int Places { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
public class GetHotelOfferQuery
{
    public TripOfferQueryParameters TripOfferQueryParameters { get; set; }
}

public class GetTransportOffersQuery
{
    public string DepartueCity { get; set; }
    public string DepartueCountry { get; set; }
    public string DestinationCity { get; set; }
    public string DestinationCountry { get; set; }
    public int Places { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
}

public class ReserveTripQuery
{
    public ReserveTripOfferParameters ReserveTripOfferParameters { get; set; }
    public Guid ReservationId { get; set; }
}
