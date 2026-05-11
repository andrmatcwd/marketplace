using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class Listing : AuditedEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(120)]
    public string? Email { get; set; }

    [MaxLength(300)]
    public string? Website { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public double Rating { get; set; }
    public int ReviewsCount { get; set; }

    [MaxLength(150)]
    public string? SellerId { get; set; }

    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Free;

    public ListingStatus Status { get; set; } = ListingStatus.Active;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; } = null!;

    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
