using System.Linq.Expressions;

namespace Domain.Primitives;

public interface IReadOnlyRepository<TEntity, in TKey>
    where TEntity : Entity<TKey>
    where TKey : IComparable
{
    Task<TResult> FirstOrDefaultAsync<TResult>(
        Expression<Func<TEntity, bool>> condition,
        params Expression<Func<TEntity, object>>[] includedPaths);
}
