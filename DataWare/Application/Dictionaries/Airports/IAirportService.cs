using Domain.Entities.Dictionaries;
using Domain.Shared;

namespace Application.Dictionaries.Airports;

public interface IAirportService
{
    public Task<Result<Airport>> GetByIATACodeAsync(string IATACode);
}