using Domain.Shared;

namespace Infrastructure.Cache;

public static class CacheErrors
{
    public static readonly Error Expired = Error.NotFound(
        "Cache.Expired",
        "Истёк срок действия кэшированных данных.");
}
