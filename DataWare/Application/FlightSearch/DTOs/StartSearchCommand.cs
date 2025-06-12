namespace Application.FlightSearch.DTOs;

public record StartSearchCommand(string ClientId, DateOnly DepartureDateUtc, string FromAirportIATACode, string ToAirportIATACode, int PassengerCount);
