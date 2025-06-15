using Domain.Primitives;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Repositories;

internal static class ServiceLocator
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IReadOnlyRepository<,>), typeof(ReadOnlyEfRepository<,>));
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        services.AddScoped<IAirlineRepository, AirlineRepository>();
        services.AddScoped<IAirportRepository, AirportRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ISearchRequestRepository, SearchRequestRepository>();

        return services;
    }
}
