using Domain.Shared;

namespace Application.FlightAggregation;

public static class FlightAggregationErrors
{
    public static readonly Error FlightsNotFound = Error.NotFound(
        "FlightAggregation.FlightsNotFound",
        "Перелёты по вашему запросу не найдены.");
}
