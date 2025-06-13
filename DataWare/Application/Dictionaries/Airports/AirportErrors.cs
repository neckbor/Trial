using Domain.Shared;

namespace Application.Dictionaries.Airports;

public static class AirportErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Airport.NotFound",
        "Аэропорт не найден.");
}
