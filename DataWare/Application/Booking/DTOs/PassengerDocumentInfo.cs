namespace Application.Booking.DTOs;

public class PassengerDocumentInfo
{
    public string DocumentTypeCode { get; set; }
    public string Number { get; set; }
    public DateOnly IssuedAt { get; set; }
    public DateOnly? ExpiresAt { get; set; }
    public string IssuedByCountryCode { get; set; }
}
