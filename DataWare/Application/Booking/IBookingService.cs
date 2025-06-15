using Application.Booking.DTOs;
using Domain.Shared;

namespace Application.Booking;

public interface IBookingService
{
    Task<Result<Guid>> CreateBookingAsync(CreateBookingCommand command);
}
