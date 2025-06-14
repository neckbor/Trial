using Infrastructure.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceLocator
{
    public static void ConfigureInfastructureServices(IServiceCollection services)
    {
        services.ConfigureBackgroundJobs();
    }
}
