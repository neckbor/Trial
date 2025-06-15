using Application.InfrastructureAbstractions;
using Infrastructure.TicketingProviders.AirTickets;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.TicketingProviders;

public static class ServiceLocator
{
    public static IServiceCollection RegisterTicketingProviders(this IServiceCollection services)
    {
        services.AddScoped<ITicketingProvider, AIrTicketsTicketingProvider>();

        return services;
    }
}
