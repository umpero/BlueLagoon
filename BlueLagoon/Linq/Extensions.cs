using AutoMapper.QueryableExtensions;
using AutoMapper;
using BlueLagoon.Kernel.Types.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlueLagoon.Linq;

public static class Extensions
{
    public static Task<T> ReturnSingleOrDefaultAsync<T>(this IQueryable<T> queryable, bool withTracking = true)
        where T : BaseEntity
        => withTracking
                ? queryable.SingleOrDefaultAsync()
                : queryable.AsNoTracking().SingleOrDefaultAsync();

    public static Task<T> ReturnSingleOrDefaultAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking = true)
        where T : BaseEntity
        => withTracking
                ? queryable.SingleOrDefaultAsync(expression)
                : queryable.AsNoTracking().SingleOrDefaultAsync(expression);

    public static Task<TProjection> ReturnSingleOrDefaultAsync<T, TProjection>(this IQueryable<T> queryable, IConfigurationProvider mapperConfiguration, bool withTracking = true)
        where T : BaseEntity
        where TProjection : class
        => withTracking
                ? queryable
                    .ProjectTo<TProjection>(mapperConfiguration)
                    .SingleOrDefaultAsync()
                : queryable
                    .AsNoTracking()
                    .ProjectTo<TProjection>(mapperConfiguration)
                    .SingleOrDefaultAsync();
    public static Task<TProjection> ReturnSingleOrDefaultAsync<T, TProjection>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, IConfigurationProvider mapperConfiguration, bool withTracking = true, object projectionParameters = null)
        where T : BaseEntity
        where TProjection : class
    {
        if (withTracking == false)
            queryable = queryable.AsNoTracking();

        queryable = queryable.Where(expression);

        return projectionParameters is { }
                        ? queryable.ProjectTo<TProjection>(mapperConfiguration, projectionParameters).SingleOrDefaultAsync()
                        : queryable.ProjectTo<TProjection>(mapperConfiguration).SingleOrDefaultAsync();
    }

    public static Task<List<T>> ReturnListAsync<T>(this IQueryable<T> queryable, bool withTracking = true)
        where T : BaseEntity
        => withTracking
                ? queryable.ToListAsync()
                : queryable.AsNoTracking().ToListAsync();

    public static Task<List<T>> ReturnListAsync<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, bool withTracking = true)
        where T : BaseEntity
        => withTracking
                ? queryable.Where(expression).ToListAsync()
                : queryable.AsNoTracking().Where(expression).ToListAsync();

    public static Task<List<TProjection>> ReturnListAsync<T, TProjection>(this IQueryable<T> queryable, Expression<Func<T, bool>> expression, IConfigurationProvider mapperConfiguration, bool withTracking = true)
        where T : BaseEntity
        where TProjection : class
        => withTracking
                ? queryable.Where(expression)
                           .ProjectTo<TProjection>(mapperConfiguration)
                           .ToListAsync()
                : queryable.AsNoTracking()
                           .Where(expression)
                           .ProjectTo<TProjection>(mapperConfiguration)
                           .ToListAsync();
}