using Application.InfrastructureAbstractions;
using Domain.Entities.Dictionaries;
using Domain.Models;
using Domain.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Cache.MemoryCache;

internal class SearchResultMemoryCache : ISearchResultCache
{
    private readonly ILogger<SearchResultMemoryCache> _logger;
    private readonly IMemoryCache _cache;
    
    private readonly TimeSpan _cacheTtl = TimeSpan.FromMinutes(10);

    private static string FlightsKey(string searchKey) => $"search:{searchKey}:flights";
    private static string StatusKey(string searchKey) => $"search:{searchKey}:status";

    public SearchResultMemoryCache(ILogger<SearchResultMemoryCache> logger, IMemoryCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public Task AddFlightsAsync(string searchKey, List<BaseFlight> flights)
    {
        var key = FlightsKey(searchKey);

        if (_cache.TryGetValue(key, out List<BaseFlight>? existingFlights))
        {
            existingFlights.AddRange(flights);

            _cache.Set(key, existingFlights, _cacheTtl);
        }
        else
        {
            _cache.Set(key, flights, _cacheTtl);
        }

        return Task.CompletedTask;
    }

    public Task<Result> CLearAsync(string searchKey)
    {
        _cache.Remove(FlightsKey(searchKey));
        _cache.Remove(StatusKey(searchKey));
        return Task.FromResult(Result.Success());
    }

    public Task<Result<List<BaseFlight>>> GetFlightsAsync(string searchKey)
    {
        if (_cache.TryGetValue(FlightsKey(searchKey), out List<BaseFlight>? flights))
        {
            return Task.FromResult(Result.Success(flights));
        }

        return Task.FromResult(Result.Failure<List<BaseFlight>>(CacheErrors.Expired));
    }

    public Task<Result<TimeSpan>> GetFlightsTtlAsync(string searchKey)
    {
        if (_cache.TryGetValue(FlightsKey(searchKey), out ICacheEntry? entry))
        {
            if (entry?.AbsoluteExpiration is DateTimeOffset expiresAt)
            {
                var ttl = expiresAt - DateTimeOffset.UtcNow;
                return Task.FromResult(Result.Success(ttl));
            }
        }

        return Task.FromResult(Result.Failure<TimeSpan>(CacheErrors.Expired));

    }

    public Task<Result<Dictionary<TicketingProvider, SearchStatus>>> GetProviderSearchStatusAsync(string searchKey)
    {
        if (_cache.TryGetValue(StatusKey(searchKey), out Dictionary<TicketingProvider, SearchStatus>? statuses))
        {
            return Task.FromResult(Result.Success(statuses));
        }

        return Task.FromResult(Result.Failure<Dictionary<TicketingProvider, SearchStatus>>(CacheErrors.Expired));
    }

    public Task SetProviderSearchStatusAsync(string searchKey, TicketingProvider provider, SearchStatus status)
    {
        var key = StatusKey(searchKey);
        var dict = _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _cacheTtl;
            return new Dictionary<TicketingProvider, SearchStatus>();
        });

        dict[provider] = status;
        return Task.CompletedTask;
    }
}
