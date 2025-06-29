﻿using Domain.Entities.Dictionaries;

namespace Domain.Models;

public class BaseSegment
{
    public string FlightNumber { get; set; }
    public Airline Airline { get; set; }
    public Airport From { get; set; }
    public Airport To { get; set; }
    public DateTime DepartureDateUtc { get; set; }
    public DateTime ArrivalDateUtc { get; set; }
    public int AvailableSeats { get; set; }
}
