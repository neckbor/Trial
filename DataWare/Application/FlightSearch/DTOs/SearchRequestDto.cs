namespace Application.FlightSearch.DTOs;

public class SearchRequestDto
{
    public string SearchResultKey { get; set; }
    public AirportDto From { get; set; }
    public AirportDto To { get; set; }
    public DateOnly DepartureDate { get; set; }
}
