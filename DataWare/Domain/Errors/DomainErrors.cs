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
    }
}
