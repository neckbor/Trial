using Application.InfrastructureAbstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cache.MemoryCache;

internal static class ServiceLocator
{
    public static void ConfigureMemoryCache(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ISearchResultCache, SearchResultMemoryCache>();
    }
}
