using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class Listing : AuditedEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;

    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";

    public string SellerId { get; set; } = string.Empty;

    public string? AddressLine { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public ListingStatus Status { get; set; } = ListingStatus.Active;

    public double RatingAverage { get; set; }
    public int ReviewsCount { get; set; }

    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
