using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Models.Admin.Services;

public sealed class AdminServicesFilterViewModel
{
    public string? Search { get; set; }
    public string? Category { get; set; }
    public string? City { get; set; }

    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }

    public bool OnlineOnly { get; set; }
    public bool OfflineOnly { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public IReadOnlyList<ServiceCategoryViewModel> Categories { get; set; } = [];
}
