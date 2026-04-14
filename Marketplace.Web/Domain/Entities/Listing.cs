namespace Marketplace.Web.Domain.Entities;

public sealed class Listing
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public double Rating { get; set; }
    public int ReviewsCount { get; set; }

    public bool IsPublished { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAtUtc { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid SubCategoryId { get; set; }
    public SubCategory? SubCategory { get; set; }

    public Guid CityId { get; set; }
    public City? City { get; set; }

    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
    public ICollection<ListingReview> Reviews { get; set; } = new List<ListingReview>();
}