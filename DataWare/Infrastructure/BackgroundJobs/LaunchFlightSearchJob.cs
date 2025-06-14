using Application.FlightSearch;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal class LaunchFlightSearchJob : IJob
{
    private readonly ILogger<LaunchFlightSearchJob> _logger;
    private readonly IFlightSearchService _flightSearchService;

    public LaunchFlightSearchJob(ILogger<LaunchFlightSearchJob> logger, IFlightSearchService flightSearchService)
    {
        _logger = logger;
        _flightSearchService = flightSearchService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _flightSearchService.LaunchFlightSearchAsync();
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Исключение при запуске поиска перелётов");
        }
    }
}
