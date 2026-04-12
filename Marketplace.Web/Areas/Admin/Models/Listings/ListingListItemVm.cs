using Marketplace.Modules.Listings.Domain.Enums.Listing;

namespace Marketplace.Web.Areas.Admin.Models.Listings;

public class ListingListItemVm
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string SubCategoryName { get; init; } = string.Empty;
    public string CityName { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = string.Empty;
    public ListingStatus Status { get; init; }
    public DateTime CreatedUtc { get; init; }
}