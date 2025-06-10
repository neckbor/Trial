using Domain.Entities;

namespace Domain.Models;

public class BaseFlight
{
    public Airport From { get; set; }
    public Airport To { get; set; }
    public List<BaseSegment> Segments { get; set; }
    public DateTime DepartureDate 
    { 
        get 
        {
            return Segments.Min(s => s.DepartureDate);
        }
        private set { }
    }
    public DateTime ArrivalDate 
    {
        get
        {
            return Segments.Max(s => s.ArrivalDate);
        }
        private set { }
    }
    public List<Airline> Airlines 
    { 
        get 
        {
            return Segments.Select(s => s.Airline).Distinct().ToList();
        }
        private set { } 
    }
    public decimal TotalPrice 
    {
        get
        {
            return Segments.Sum(s => s.FareDetails.TotalPrice);
        }
        private set { }
    }
    public int AvailableSeats
    {
        get
        {
            return Segments.Min(s => s.AvailableSeats);
        }

        private set { }
    }
}
