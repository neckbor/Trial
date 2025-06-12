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
}
