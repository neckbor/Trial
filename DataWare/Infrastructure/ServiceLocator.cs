using Infrastructure.BackgroundJobs;
using Infrastructure.TicketingProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceLocator
{
    public static void ConfigureInfastructureServices(IServiceCollection services)
    {
        services.ConfigureBackgroundJobs();

        services.RegisterTicketingProviders();
    }
}
