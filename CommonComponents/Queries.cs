using System;
using System.Collections.Generic;
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

public class ReserveHotelQuery
{
    public ReserveTripOfferParameters ReserveTripOfferParameters { get; set; }

    public Guid ReservationId { get; set; }
}

public class ReserveTransportQuery
{
    public long DepartueTransportID { get; set; }
    public long ReturnTransportID { get; set; }
    public int Places { get; set; }
    public Guid ReservationId { get; set; }
    public ReserveTripOfferParameters ReserveTripOfferParameters { get; set; }
    public string HotelCity { get; set; }
}

public class RollbackHotelReservationQuery
{
    public Guid TripReservationId { get; set; }
}

public class RollbackTransportReservationQuery
{
    public Guid TripReservationId { get; set; }

}
public class PaymentQuery
{
    public Guid ReservationId { get; set; }
    public string CardNumber { get; set; }
    public string Username { get; set; }

}

public class PayForTripQuery
{
    public Guid ReservationId { get; set; }
    public string CardNumber { get; set; }
    public int Price { get; set; }
}

public class ConfirmHotelOrderQuery
{
    public Guid ReservationId { get; set; }
}

public class ConfirmTransportOrderQuery
{
    public Guid ReservationId { get; set; }
}

public class GetDestinationsQuery
{
    
}

public class GetUserTripsQuery
{
    public string Username { get; set; }
}

public class GetUserTripsTransportQuery
{
    public string Username { get; set; }
}

public class GetUserTripsHotelsQuery
{
    public string Username { get; set; }
}

public class CreateUserTripQuery
{
    public Guid id { get; set; }
    public string RoomTypeName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string FoodTypeName { get; set; }
    public string HotelName { get; set; }
    public string TransportTypeName { get; set; }
    public string username { get; set; }
    public int Persons { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class NewReservationCompleatedQuery
{
    public string CountryName { get; set; }
    public string HotelName { get; set; }
    public string TransportType { get; set; }
    public string NameOfRoom { get; set; }
    public DateTime EventDate { get; set; }
}
public class NotifyAboutNewPopularCountryQuery
{
    public string CountryName { get; set; }
}

public class NotifyAboutTripPurchaseQuery
{
    public string UserName { get; set; }
    public int HotelId { get; set; }
}

public class GetNotificationAboutPopularCountryQuery
{
}

public class GetNotificationAboutPopularTripConfigurationQuery
{
}

public class NotifyAboutNewPopularTripConfigQuery
{
    public string Hotel { get; set; }
    public string Room { get; set; }
    public string Transport { get; set; }
}

public class ChangeHotelAvailabilityQuery
{
    public List<short> IdentifierList { get; set; }
    public short ChangeQuantity { get; set; }
    public short HotelId { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }

}

public class ChangeTransportPlacesQuery
{
    public int ChangePlaces { get; set; }
    public long TransportId { get; set; }
    public long DestinationPlacesId { get; set; }
    public long SourcePlacesId { get; set; }
    public string Transporttype { get; set; } = null!;
    public DateTime Transportdate { get; set; }
}