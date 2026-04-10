using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Models.Listings;

public sealed class ListingsPageViewModel
{
    public ListingsFilterRequest Filters { get; set; } = new();
    public IReadOnlyList<CategoryViewModel> Categories { get; set; } = [];
    public PagedResult<ListingViewModel> Results { get; set; } = new();
}