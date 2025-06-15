using Domain.Entities.Dictionaries;

namespace Application.Booking.DTOs;

public class PassengerInfo
{
    public string FisrtName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string PassportNumber { get; set; }
    public string CountryCitizenshipCode { get; set; }
}
