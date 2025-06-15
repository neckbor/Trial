using Domain.Entities.Dictionaries;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Dictionaries.Airlines;

internal class AirlineService : IAirlineService
{
    private readonly ILogger<AirlineService> _logger;
    private readonly IAirlineRepository _airlineRepository;

    public AirlineService(ILogger<AirlineService> logger, IAirlineRepository airlineRepository)
    {
        _logger = logger;
        _airlineRepository = airlineRepository;
    }

    public async Task<Result<Airline>> GetByICAOCodeAsync(string ICAOCode)
    {
        var airline = await _airlineRepository.FirstOrDefaultAsync<Airline>(a => a.ICAOCode.Equals(ICAOCode.ToUpperInvariant()));
        if (airline is null)
        {
            _logger.LogWarning("Авиакомпания по коду ICAO {ICAOCOde} не найдена", ICAOCode);

            return Result.Failure<Airline>(AirlineErrors.NotFound);
        }

        return airline;
    }
}
