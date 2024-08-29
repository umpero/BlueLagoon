using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlueLagoon.Kernel.Types.Abstractions;
using BlueLagoon.Pagination.Request;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BlueLagoon.Pagination.Response;

public class PaginatedList<TDto> where TDto : class
{
    private const int _firstPageNumber = 1; 

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public IEnumerable<TDto> Items { get; private set; }

    public bool HasPrevious => PageNumber > _firstPageNumber;

    public bool HasNext => PageNumber < TotalPages;

    public static async Task<PaginatedList<TDto>> GetPaginatedPageAsync<TEntity>(IQueryable<TEntity> queryable, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters = null)
        where TEntity : BaseEntity

    {
        var count = await queryable.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        queryable = queryable
                        .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                        .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                        .Take((int)paginationParameters.PageSize);

        var result =
            projectionParameters is { } 
                ? await queryable.ProjectTo<TDto>(configurationProvider, projectionParameters)
                                 .ToListAsync() 
                : await queryable.ProjectTo<TDto>(configurationProvider)
                                 .ToListAsync();

        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = result
        };
    }

    public static async Task<PaginatedList<TDto>> GetPaginatedPageWithOrderingAfterProjectionAsync<TEntity>(IQueryable<TEntity> queryable, PaginationParameters paginationParameters, IConfigurationProvider configurationProvider, object projectionParameters = null) 
        where TEntity : BaseEntity
    {
        var count = await queryable.CountAsync();

        paginationParameters = PaginationParameters.CreatePaginationParametersIfNotExists(paginationParameters);

        var result =
            projectionParameters is { }
                ? await queryable
                            .ProjectTo<TDto>(configurationProvider, projectionParameters)
                            .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                            .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                            .Take((int)paginationParameters.PageSize)
                            .ToListAsync()
                : await queryable
                            .ProjectTo<TDto>(configurationProvider)
                            .OrderBy(paginationParameters.OrderByColumn + " " + paginationParameters.SortBy)
                            .Skip(((int)paginationParameters.PageNumber - 1) * (int)paginationParameters.PageSize)
                            .Take((int)paginationParameters.PageSize)
                            .ToListAsync();

        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = result
        };
    }

    public static PaginatedList<TDto> GetPaginatePageFromList(IEnumerable<TDto> collection, int count, PaginationParameters paginationParameters)
    {
        return new PaginatedList<TDto>
        {
            PageSize = (int)paginationParameters.PageSize,
            PageNumber = (int)paginationParameters.PageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize),
            TotalCount = count,
            Items = collection
        };
    }

    public static PaginatedList<TDto> GetEmptyPaginatedList()
        => new()
        {
            PageSize = default,
            PageNumber = default,
            TotalPages = default,
            TotalCount = default,
            Items = []
        };
}