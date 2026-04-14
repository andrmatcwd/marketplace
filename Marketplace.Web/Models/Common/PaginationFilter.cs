namespace Marketplace.Web.Models.Common;

public sealed class PaginationFilter
{
    private const int MaxPageSize = 60;

    private int _page = 1;
    private int _pageSize = 12;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 12 : Math.Min(value, MaxPageSize);
    }
}