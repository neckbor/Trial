using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache;

internal static class ServiceLocator
{
    public static IServiceCollection ConfigureCache(this IServiceCollection services)
    {
        MemoryCache.ServiceLocator.ConfigureMemoryCache(services);

        return services;
    }
}
