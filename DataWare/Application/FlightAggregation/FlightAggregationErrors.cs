using Domain.Shared;

namespace Application.FlightAggregation;

public static class FlightAggregationErrors
{
    public static readonly Error FlightsNotFound = Error.NotFound(
        "FlightAggregation.FlightsNotFound",
        "Перелёты по вашему запросу не найдены.");

    public static readonly Error FlightByIdNotFound = Error.NotFound(
        "FlightAggregation.FlightsNotFound",
        "Запрашиваемый перелёт не найден. Повторите поиск.");
}
