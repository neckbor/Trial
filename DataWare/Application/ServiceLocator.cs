using Application.Booking;
using Application.Dictionaries.Airlines;
using Application.Dictionaries.Airports;
using Application.Dictionaries.Countries;
using Application.FlightAggregation;
using Application.FlightSearch;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceLocator
{
    public static void RegisterApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IFlightSearchService, FlightSearchService>();
        services.AddScoped<IFlightAggregator, FlightAggregator>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IAirlineService, AirlineService>();
        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<ICountryService, CountryService>();
    }
}
