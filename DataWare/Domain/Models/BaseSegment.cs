using Domain.Entities;

namespace Domain.Models;

public class BaseSegment
{
    public Airline Airline { get; set; }
    public Airport From { get; set; }
    public Airport To { get; set; }
    public DateTime DepartureDateUtc { get; set; }
    public DateTime ArrivalDateUtc { get; set; }
}
