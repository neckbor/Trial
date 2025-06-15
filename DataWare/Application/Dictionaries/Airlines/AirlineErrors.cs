using Domain.Shared;

namespace Application.Dictionaries.Airlines;

public static class AirlineErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Airline.NotFound",
        "Авиакомпания не найдена.");
}
