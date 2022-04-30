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
