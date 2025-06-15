namespace Application.Booking.DTOs;

public record CreateBookingCommand(Guid SearchRequestId, string FlightId);
