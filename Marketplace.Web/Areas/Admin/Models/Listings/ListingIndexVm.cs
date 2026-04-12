using Marketplace.Modules.Listings.Domain.Enums.Listing;

namespace Marketplace.Web.Areas.Admin.Models.Listings;

public class ListingIndexVm
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }
    public int? SubCategoryId { get; set; }
    public int? LocationId { get; set; }
    public ListingStatus? Status { get; set; }

    public IReadOnlyCollection<ListingListItemVm> Items { get; init; } = [];
}