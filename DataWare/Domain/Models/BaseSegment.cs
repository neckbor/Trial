using Domain.Entities;

namespace Domain.Models;

public class BaseSegment
{
    public string FlightNumber { get; set; }
    public Airport From { get; set; }
    public Airport To { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public Airline Airline { get; set; }
    public int AvailableSeats { get; set; }
    public FareDetailsDto FareDetails { get; set; }
}
