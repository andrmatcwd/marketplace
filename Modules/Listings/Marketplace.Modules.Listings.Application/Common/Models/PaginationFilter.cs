namespace Marketplace.Modules.Listings.Application.Common.Models;

public class PaginationFilter
{
    private const int MaxPageSize = 100;

    public int Page { get; init; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
