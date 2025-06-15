using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class BookingStatus
    {
        public static readonly Error NotFound = Error.NotFound(
            "BookingStatus.NotFound",
            "Не найден статус бронирования.");
    }

    public static class DocumentType
    {
        public static readonly Error NotFound = Error.NotFound(
            "BookingStatus.DocumentType",
            "Не найден тип документа.");
    }

    public static class FlightSegment
    {
        public static readonly Error InvalidSegmentDates = Error.Validation(
            "FlightSegment.InvalidSegmentDates",
            "Некорректные даты отправления/прибытия.");
    }

    public static class SearchStatus
    {
        public static readonly Error NotFound = Error.NotFound(
            "SearchStatus.NotFound",
            "Не найден статус поиска.");
    }

    public static class SearchRequest
    {
        public static readonly Error InvalidDepartureDate = Error.Validation(
            "SearchRequest.InvalidDepartureDate",
            "Некорректная дата отправления.");

        public static readonly Error SameAirports = Error.Validation(
            "SearchRequest.SameAirports",
            "Одинаковые аэропорты вылета и прилёта.");

        public static readonly Error InvalidPassengerCount = Error.Validation(
            "SearchRequest.InvalidPassengerCount",
            "Некорректное значение количества пассажиров.");
    }

    public static class Booking
    {
        public static readonly Error AlreadyHasFlight = Error.Conflict(
            "Booking.AlreadyHasFlight",
            "К брониорванию уже привязан перелёт.");
    }

    public static class Passenger
    {
        public static readonly Error FirstnameIsEmpty = Error.Validation(
            "Passenger.FirstnameIsEmpty",
            "Необходимо задать имя пассажира.");

        public static readonly Error LastnameIsEmpty = Error.Validation(
            "Passenger.LastnameIsEmpty",
            "Необходимо задать фамилию пассажира.");

        public static readonly Error InvalidDateOfBirth = Error.Validation(
            "Passenger.InvalidDateOfBirth",
            "Некорректное значение даты рождения.");

        public static readonly Error PassportNumberIsEmpty = Error.Validation(
            "Passenger.PassportNumberIsEmpty",
            "Необходимо ввести номер паспорта.");
    }

    public static class PassengerDocument
    {
        public static readonly Error NumberIsEmpty = Error.Validation(
            "PassengerDocument.NumberIsEmpty",
            "Необходимо задать номер документа.");

        public static readonly Error InvalidIssuedAtDate = Error.Validation(
            "PassengerDocument.InvalidIssuedAtDate",
            "Некорректная дата выдачи документа.");

        public static readonly Error InvalidExpiresAtDate = Error.Validation(
            "PassengerDocument.InvalidIssuedAtDate",
            "Некорректный срок действия документа.");
    }
}
