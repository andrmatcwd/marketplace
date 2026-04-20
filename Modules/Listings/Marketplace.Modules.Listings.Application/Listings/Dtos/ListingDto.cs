using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Application.Listings.Dtos;

public class ListingDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? SellerId { get; set; }

    public string? AddressLine { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;
    public ListingStatus Status { get; set; } = ListingStatus.Active;

    public double? RatingAverage { get; set; }
    public int ReviewsCount { get; set; }

    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;

    public string SubCategoryName { get; set; } = string.Empty;
    public string SubCategorySlug { get; set; } = string.Empty;

    public string CityName { get; set; } = string.Empty;
    public string CitySlug { get; set; } = string.Empty;

    public IList<string> ImageUrls { get; set; } = new List<string>();

    public DateTime CreatedUtc { get; set; }
}
