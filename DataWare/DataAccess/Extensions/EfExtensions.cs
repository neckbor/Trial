using Microsoft.EntityFrameworkCore;

namespace DataAccess.Extensions;

internal static class EfExtensions
{
    public static Task<List<TSource>> ToListAsyncSafe<TSource>(
        this IQueryable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source is IAsyncEnumerable<TSource>
            ? source.ToListAsync()
            : Task.FromResult(source.ToList());
    }
}
