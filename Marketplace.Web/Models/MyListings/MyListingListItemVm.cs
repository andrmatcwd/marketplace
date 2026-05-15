using Marketplace.Modules.Listings.Domain.Enums.Listing;

namespace Marketplace.Web.Models.MyListings;

public class MyListingListItemVm
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public ListingStatus Status { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string? PrimaryImageUrl { get; set; }
    public DateTime CreatedUtc { get; set; }
}
