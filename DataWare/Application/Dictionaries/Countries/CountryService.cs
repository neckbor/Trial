using Domain.Entities.Dictionaries;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Dictionaries.Countries;

internal class CountryService : ICountryService
{
    private readonly ILogger<CountryService> _logger;
    private readonly ICountryRepository _countryRepository;

    public CountryService(ILogger<CountryService> logger, ICountryRepository countryRepository)
    {
        _logger = logger;
        _countryRepository = countryRepository;
    }

    public async Task<Result<Country>> GetByCodeAsync(string code)
    {
        var country = await _countryRepository.FirstOrDefaultAsync<Country>(c => c.Code.Equals(code.ToUpperInvariant()));

        if (country is null) 
        {
            _logger.LogWarning("Не найдена страна по коду {CountryCode}", code);

            return Result.Failure<Country>(CountryErrors.NotFound);
        }

        return country;
    }
}
