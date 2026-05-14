namespace Marketplace.Modules.Appointments.Application.Common.Models;

public class PaginationFilter : BaseFilter
{
    private int _pageSize = 20;

    public int Page { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Clamp(value, 1, 100);
    }
}
