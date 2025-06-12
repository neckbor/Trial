using Domain.Entities.Dictionaries;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Dictionaries.Airports;

internal class AirportService : IAirportService
{
    private readonly IAirportRepository _airportRepository;

    public AirportService(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<Result<Airport>> GetByIATACodeAsync(string IATACode)
    {
        var airport = await _airportRepository.FirstOrDefaultAsync<Airport>(a => a.IATACode.Equals(IATACode.ToUpperInvariant()));
        if (airport is null)
        {
            return Result.Failure<Airport>(Errors.NotFound);
        }

        return airport;
    }
}
