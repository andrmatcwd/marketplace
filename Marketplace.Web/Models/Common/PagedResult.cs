namespace Marketplace.Web.Models.Common;

public sealed class PagedResult<T>
{
    public IReadOnlyCollection<T> Items { get; set; } = Array.Empty<T>();

    public int Page { get; set; }
    public int PageSize { get; set; }

    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}