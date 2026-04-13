using System;
using Marketplace.Web.Models.Common;

namespace Marketplace.Web.Models.Listings;

public class ListingDetailsPageVm
{
    // Identity
    public int Id { get; init; }

    // URL structure
    public string CityName { get; init; } = default!;
    public string CitySlug { get; init; } = default!;

    public string CategoryName { get; init; } = default!;
    public string CategorySlug { get; init; } = default!;

    public string SubCategoryName { get; init; } = default!;
    public string SubCategorySlug { get; init; } = default!;

    public string ListingSlug { get; init; } = default!;

    // Main content
    public string Title { get; init; } = default!;
    public string H1 { get; init; } = default!;
    public string? ShortDescription { get; init; }
    public string? Description { get; init; }

    // Business info
    public string? ProviderName { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Website { get; init; }
    public string? Address { get; init; }

    // Geo
    public string? CityDistrict { get; init; }
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }

    // Trust / rating
    public decimal? Rating { get; init; }
    public int ReviewsCount { get; init; }
    public bool IsVerified { get; init; }

    // Media
    public string? MainImageUrl { get; init; }
    public IReadOnlyCollection<ListingImageVm> Images { get; init; }
        = Array.Empty<ListingImageVm>();

    // Service info
    public IReadOnlyCollection<string> ServiceFeatures { get; init; }
        = Array.Empty<string>();

    // Optional reviews preview
    public IReadOnlyCollection<ListingReviewVm> Reviews { get; init; }
        = Array.Empty<ListingReviewVm>();

    // Related content
    // public IReadOnlyCollection<ListingCardVm> RelatedListings { get; init; }
    //     = Array.Empty<ListingCardVm>();

    // Navigation
    public IReadOnlyCollection<BreadcrumbItemVm> Breadcrumbs { get; init; }
        = Array.Empty<BreadcrumbItemVm>();
}
