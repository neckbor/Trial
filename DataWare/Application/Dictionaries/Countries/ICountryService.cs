using Domain.Entities.Dictionaries;
using Domain.Shared;

namespace Application.Dictionaries.Countries;

public interface ICountryService
{
    Task<Result<Country>> GetByCodeAsync(string code);
}
