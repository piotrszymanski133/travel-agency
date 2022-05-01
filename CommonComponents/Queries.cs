using System;

namespace CommonComponents;

public class GetTripsQuery
{
    public string Country { get; set; }
}

public class GetHotelsQuery
{
    public string? Country { get; set; }
    public string? City { get; set; }
}

public class GetTransportQuery
{
    public string DestinationCity { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
}
