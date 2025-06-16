using Domain.Entities.Dictionaries;
using Domain.Shared;

namespace Infrastructure.TicketingProviders;

public static class TicketingProviderErrors
{
    public static readonly Func<TicketingProvider, Error> SearchFailed = provider => Error.Failure(
        $"TicketingProvider.{provider.Code}.SearchFailed",
        $"Непредвиденная ошибка при поиске перелётов у провайдера {provider.Name}");

    public static readonly Func<TicketingProvider, Error> FlightsNotFound = provider => Error.NotFound(
        $"TicketingProvider.{provider.Code}.FlightsNotFound",
        $"Не найдено перелётов у провайдера {provider.Name}");

    public static readonly Func<TicketingProvider, Error> ProviderUnavailable = provider => Error.Failure(
        $"TicketingProvider.{provider.Code}.Unavailable",
        $"Не удалось получить данные от провайдера {provider.Name}");

    public static readonly Func<TicketingProvider, Error> ParsingFailed = provider => Error.Validation(
        $"TicketingProvider.{provider.Code}.ParsingFailed",
        $"Не удалось распарсить данные от провайдера {provider.Name}");
}

