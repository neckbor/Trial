using Domain.Entities.Dictionaries;

namespace Domain.Entities;

public class Passenger
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? MiddleName { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Sex Sex { get; private set; }
    public PassengerDocument Document { get; private set; }

    private Passenger(Guid id,
        string name,
        string surname,
        string? middleName,
        DateOnly dateOfBirth,
        Sex sex)
    {
        Id = id;
        Name = name;
        Surname = surname;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        Sex = sex;
    }

    public static Passenger Create(
        string name,
        string surname,
        string? middleName,
        DateOnly dateOfBirth,
        Sex sex)
    {
        return new(Guid.NewGuid(), name, surname, middleName, dateOfBirth, sex);
    }

    public void AddDocument(
        DocumentType documentType,
        string number,
        DateOnly issuedDate,
        DateOnly? validUntil,
        Country issuedBy)
    {
        Document = PassengerDocument.Create(this, documentType, number, issuedDate, issuedDate, issuedBy);
    }
}
