namespace WebApi.Contracts.FlightSearch;

public class CreateFlightSearchRequest
{
    public DateOnly DepartureDate { get; set; }
    public string FromIATA { get; set; }
    public string ToIATA { get; set; }
    public int PassengerCount { get; set; }
}
