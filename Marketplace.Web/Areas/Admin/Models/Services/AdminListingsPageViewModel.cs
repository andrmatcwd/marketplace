using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Areas.Admin.Models.Listings;

public sealed class AdminListingsPageViewModel
{
    public AdminListingsFilterViewModel Filter { get; set; } = new();
    public IReadOnlyList<ListingItemViewModel> Items { get; set; } = [];
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
