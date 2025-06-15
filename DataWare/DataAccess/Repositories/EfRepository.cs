using AutoMapper;
using Domain.Primitives;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

internal class EfRepository<TEntity, TKey> : ReadOnlyEfRepository<TEntity, TKey>, IRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : IComparable
{
    public EfRepository(
        DataWareDbContext context, 
        IMapper mapper, 
        IConfigurationProvider mapperConfiguration) : 
        base(context, mapper, mapperConfiguration)
    {
    }

    public virtual async Task InsertAsync(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }
}
