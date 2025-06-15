using Domain.Shared;

namespace Application.Booking;

public static class BookingErrors
{
    public static readonly Error Failed = Error.Failure(
        "Booking.Failed",
        "Произошла ошибка при поптыке забронировать перелёт. Попробуйте снова или повторите поиск.");

    public static readonly Error PassengerCountChanged = Error.Conflict(
        "Booking.PassengerCountChanged",
        "Передано другое количество пассажиров.");
}
