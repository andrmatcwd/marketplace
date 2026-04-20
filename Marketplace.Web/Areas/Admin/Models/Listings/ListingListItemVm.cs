using Marketplace.Modules.Listings.Domain.Enums.Listing;

namespace Marketplace.Web.Areas.Admin.Models.Listings;

public class ListingListItemVm
{
    public int Id { get; init; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string SubCategoryName { get; init; } = string.Empty;
    public string CityName { get; init; } = string.Empty;
    public ListingStatus Status { get; init; }
    public string Url { get; init; } = default!;
    public DateTime CreatedUtc { get; init; }
}