using Domain.Entities.Dictionaries;

namespace Domain.Entities;

public class PassengerDocument
{
    public Passenger Passenger { get; private set; }
    public DocumentType Type { get; private set; }
    public string Number { get; private set; }
    public DateOnly IssuedAt { get; private set; }
    public DateOnly? ExpiresAt { get; private set; }
    public Country IssuedBy { get; private set; }

    private PassengerDocument(
        Passenger passenger,
        DocumentType type,
        string number,
        DateOnly issuedAt,
        DateOnly? expiresAt,
        Country issuedBy)
    {
        Passenger = passenger;
        Type = type;
        Number = number;
        IssuedAt = issuedAt;
        ExpiresAt = expiresAt;
        IssuedBy = issuedBy;
    }

    internal static PassengerDocument Create(
        Passenger passenger,
        DocumentType type,
        string number,
        DateOnly issuedAt,
        DateOnly? expiresAt,
        Country issuedBy)
    {
        return new(
            passenger,
            type,
            number,
            issuedAt,
            expiresAt,
            issuedBy);
    }
}
