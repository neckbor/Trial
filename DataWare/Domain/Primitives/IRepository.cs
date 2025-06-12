namespace Domain.Primitives;

public interface IRepository<TEntity, in TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : IComparable
{
    Task InsertAsync(TEntity entity);
}
