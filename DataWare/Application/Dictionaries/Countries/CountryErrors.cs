using Domain.Shared;

namespace Application.Dictionaries.Countries;

public static class CountryErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "Country.NotFound",
        "СТрана не найдена.");
}
