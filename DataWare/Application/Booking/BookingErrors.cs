using Domain.Shared;

namespace Application.Booking;

public static class BookingErrors
{
    public static readonly Error Failed = Error.Failure(
        "Booking.Failed",
        "Произошла ошибка при поптыке забронировать перелёт. Попробуйте снова или повторите поиск.");
}
