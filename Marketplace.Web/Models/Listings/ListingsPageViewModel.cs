namespace Marketplace.Web.Models.Listings;

public sealed class ListingsPageViewModel
{
    public ListingsFilterRequest Filters { get; set; } = new();
    public IReadOnlyList<ListingCategoryViewModel> Categories { get; set; } = [];
    public PagedResult<ListingItemViewModel> Results { get; set; } = new();
}