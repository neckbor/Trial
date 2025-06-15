using Infrastructure.BackgroundJobs;
using Infrastructure.TicketingProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceLocator
{
    public static void RegisterInfastructureServices(IServiceCollection services)
    {
        services.ConfigureBackgroundJobs();

        services.RegisterTicketingProviders();
    }
}
