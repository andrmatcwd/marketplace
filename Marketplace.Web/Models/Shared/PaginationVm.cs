namespace Marketplace.Web.Models.Shared;

public sealed class PaginationVm
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }

    public string? PrevUrl { get; set; }
    public string? NextUrl { get; set; }

    public IReadOnlyCollection<PaginationItemVm> Items { get; set; } = Array.Empty<PaginationItemVm>();

    public bool HasPagination => TotalPages > 1;
}

public sealed class PaginationItemVm
{
    public int? Page { get; set; }
    public string? Url { get; set; }
    public bool IsActive { get; set; }
    public bool IsEllipsis { get; set; }
}