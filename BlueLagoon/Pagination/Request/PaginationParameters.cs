using Common.Pagination;

namespace BlueLagoon.Pagination.Request;

public record PaginationParameters
{
    private const int _defaultMaxPageSize = 100;
    private const int _firstPageNumber = 1;

    private int? _pageSize;
    public int? PageSize
    {
        get => _pageSize is null ? _defaultMaxPageSize : _pageSize;

        set
        {
            _pageSize =
                _pageSize is <= default(int) or null
                    ? _defaultMaxPageSize
                    : value;
        }
    }

    private int? _pageNumber;
    public int? PageNumber
    {
        get => _pageNumber is null ? _firstPageNumber : _pageNumber;

        set
        {
            _pageNumber =
                _pageNumber is < 1 or null
                    ? _firstPageNumber
                    : value;
        }
    }

    private string _orderByColumn;
    public string OrderByColumn
    {
        get => string.IsNullOrEmpty(_orderByColumn) ? DefaultOrderColumn.CreatedAt : _orderByColumn;

        set
        {
            _orderByColumn =
                string.IsNullOrEmpty(_orderByColumn)
                            ? DefaultOrderColumn.CreatedAt
                            : value;
        }
    }

    private string _sortBy;
    public string SortBy
    {
        get => _sortBy is null || (_sortBy != SortOrder.Ascending && _sortBy != SortOrder.Descending)
                                                            ? SortOrder.Ascending
                                                            : _sortBy;

        set
        {
            _sortBy =
                _sortBy != SortOrder.Ascending && _sortBy != SortOrder.Descending
                                                            ? _sortBy = SortOrder.Ascending
                                                            : value;
        }
    }

    public static PaginationParameters CreatePaginationParametersIfNotExists(PaginationParameters paginationParameters)
    {
        paginationParameters ??= new PaginationParameters();
        paginationParameters.PageNumber ??= default(int?);
        paginationParameters.PageSize ??= default(int?);
        paginationParameters.OrderByColumn ??= default;
        paginationParameters.SortBy ??= default;

        return paginationParameters;
    }
}
