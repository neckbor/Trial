namespace Application.FlightSearch.DTOs;

public record StartSearchCommand(string ClientId, DateOnly DepartureDate, string FromAirportIATACode, string ToAirportIATACode, int PassengerCount);
