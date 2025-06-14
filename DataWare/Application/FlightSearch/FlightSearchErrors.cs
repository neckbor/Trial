using Domain.Shared;

namespace Application.FlightSearch;

public static class FlightSearchErrors
{
    public static readonly Error NotFound = Error.NotFound(
        "FlightSearch.NotFound",
        "Не найден запрос на поиск перелётов.");

    public static readonly Error GetResultFailed = Error.Failure(
        "FlightSearch.GetResultFailed",
        "Ошибка получения результатов поиска. Повторите попытку или сделайте поиск заново.");
}
