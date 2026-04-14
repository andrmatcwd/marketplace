using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Models.Listings;

public sealed class ListingDetailsPageVm
{
    public string Culture { get; set; } = "uk";

    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }
    public string? CategoryUrl { get; set; }

    public string? SubCategoryName { get; set; }
    public string? SubCategorySlug { get; set; }
    public string? SubCategoryUrl { get; set; }

    public string? CityName { get; set; }
    public string? CitySlug { get; set; }
    public string? CityUrl { get; set; }

    public ListingContactVm Contact { get; set; } = new();
    public ListingGalleryVm Gallery { get; set; } = new();

    public double Rating { get; set; }
    public int ReviewsCount { get; set; }

    public string? Address { get; set; }
    public string? WorkingHours { get; set; }

    public bool IsVerified { get; set; }
    public bool IsFeatured { get; set; }

    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; set; } = Array.Empty<BreadcrumbItemVm>();
    public IReadOnlyCollection<ListingReviewVm> Reviews { get; set; } = Array.Empty<ListingReviewVm>();
    public IReadOnlyCollection<RelatedListingVm> RelatedListings { get; set; } = Array.Empty<RelatedListingVm>();

    public bool HasRating => Rating > 0;
    public bool HasReviews => Reviews.Count > 0;
    public bool HasRelatedListings => RelatedListings.Count > 0;
    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);
    public bool HasShortDescription => !string.IsNullOrWhiteSpace(ShortDescription);
    public bool HasAddress => !string.IsNullOrWhiteSpace(Address);
    public string RatingFormatted => Rating > 0 ? Rating.ToString("0.0") : string.Empty;

    public double? Latitude { get; set; }
public double? Longitude { get; set; }

public bool HasCoordinates => Latitude.HasValue && Longitude.HasValue;
}