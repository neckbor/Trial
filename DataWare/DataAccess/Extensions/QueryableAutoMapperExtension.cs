using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Expressions;

namespace DataAccess.Extensions;

internal static class QueryableAutoMapperExtension
{
    /// <summary>
    /// Extension method to project from a queryable using the provided mapping engine,
    /// if the source type does not match the destination type,
    /// otherwise, returns the source type as it is
    /// </summary>
    /// <remarks>Projections are only calculated once and cached</remarks>
    /// <typeparam name="TDestination">Destination type</typeparam>
    /// <param name="source">Queryable source</param>
    /// <param name="configuration">Mapper configuration</param>
    /// <param name="membersToExpand">Explicit members to expand</param>
    /// <returns>Expression to project into</returns>
    public static IQueryable<TDestination> MapTo<TDestination>(
        this IQueryable source,
        IConfigurationProvider configuration,
        params Expression<Func<TDestination, object>>[] membersToExpand)
        => source.ElementType == typeof(TDestination)
            ? (IQueryable<TDestination>)source
            : source.ProjectTo(configuration, membersToExpand);
}
