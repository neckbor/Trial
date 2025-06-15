using Domain.Entities.Dictionaries;
using Domain.Errors;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class Passenger : Entity<Guid>
{
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string? Middlename { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string PassportNumber { get; private set; }
    public Country Country { get; private set; }

    private Passenger(
        Guid id,
        string firstname,
        string lastname,
        string? middlename,
        DateOnly dateOfBirth,
        Gender gender,
        string passportNumber,
        Country country)
    {
        Id = id;
        Firstname = firstname;
        Lastname = lastname;
        Middlename = middlename;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        PassportNumber = passportNumber;
        Country = country;
    }

    public static Result<Passenger> Create(
        string firstname,
        string lastname,
        string? middlename,
        DateOnly dateOfBirth,
        Gender gender,
        string passportNumber,
        Country country)
    {
        if (string.IsNullOrWhiteSpace(firstname))
        {
            return Result.Failure<Passenger>(DomainErrors.Passenger.FirstnameIsEmpty);
        }

        if (string.IsNullOrWhiteSpace(lastname)) 
        {
            return Result.Failure<Passenger>(DomainErrors.Passenger.LastnameIsEmpty);
        }

        if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            return Result.Failure<Passenger>(DomainErrors.Passenger.InvalidDateOfBirth);
        }

        if (string.IsNullOrWhiteSpace(passportNumber))
        {
            return Result.Failure<Passenger>(DomainErrors.Passenger.PassportNumberIsEmpty);
        }

        return new Passenger(Guid.NewGuid(), firstname, lastname, middlename, dateOfBirth, gender, passportNumber, country);
    }
}
