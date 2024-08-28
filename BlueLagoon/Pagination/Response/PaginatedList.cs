using System.Collections.Generic;

namespace BlueLagoon.Pagination.Response;

public sealed class PaginatedList<TDto> where TDto : class
{
    private const int _firstPageNumber = 1; 

    public int PageNumber { get; set; }

    public int TotalPages { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public IEnumerable<TDto> Items { get; private set; }

    public bool HasPrevious => PageNumber > _firstPageNumber;

    public bool HasNext => PageNumber < TotalPages;
}
