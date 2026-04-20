using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Catalog;

public sealed class CatalogFilterVm
{
    public string? Search { get; set; }
    public string? City { get; set; }
    public string? Category { get; set; }
    public string? Sort { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 3;

    public string ResetUrl { get; set; } = "/catalog";

    public IReadOnlyCollection<FilterOptionVm> Cities { get; set; } = Array.Empty<FilterOptionVm>();
    public IReadOnlyCollection<FilterOptionVm> Categories { get; set; } = Array.Empty<FilterOptionVm>();
    public IReadOnlyCollection<FilterOptionVm> SortOptions { get; set; } = Array.Empty<FilterOptionVm>();
}