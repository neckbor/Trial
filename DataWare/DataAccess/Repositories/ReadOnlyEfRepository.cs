using AutoMapper;
using DataAccess.Extensions;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

internal class ReadOnlyEfRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : IComparable
{
    protected readonly DataWareDbContext Context;
    protected readonly IMapper Mapper;
    protected readonly IConfigurationProvider MapperConfiguration;

    public ReadOnlyEfRepository(DataWareDbContext context, IMapper mapper, IConfigurationProvider mapperConfiguration)
    {
        Context = context;
        Mapper = mapper;
        MapperConfiguration = mapperConfiguration;
    }

    public virtual Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> condition, params Expression<Func<TEntity, object>>[] includedPaths)
    {
        return Query(includedPaths)
            .Where(condition)
            .MapTo<TResult>(MapperConfiguration)
            .FirstOrDefaultAsync();
    }

    public virtual Task<TResult> GetByKeyAsync<TResult>(TKey id)
    {
        return Query()
            .Where(e => e.Id.Equals(id))
            .MapTo<TResult>(MapperConfiguration)
            .FirstOrDefaultAsync();
    }

    public virtual Task<List<TResult>> SearchAsync<TResult>(Expression<Func<TEntity, bool>> condition)
    {
        return Context.Set<TEntity>()
            .Where(condition)
            .MapTo<TResult>(MapperConfiguration)
            .ToListAsyncSafe();
    }

    protected virtual IQueryable<TEntity> Query(
        params Expression<Func<TEntity, object>>[] includedPaths)
    {
        return Context.Set<TEntity>().IncludeByPath(includedPaths);
    }
}
