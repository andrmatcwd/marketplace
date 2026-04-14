using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Mappings;

public sealed class ListingVmMapper : IListingVmMapper
{
    public ListingDetailsPageVm MapDetails(Listing entity, string culture, IReadOnlyCollection<RelatedListingVm>? relatedListings = null)
    {
        var citySlug = entity.City?.Slug;
        var categorySlug = entity.Category?.Slug;
        var subCategorySlug = entity.SubCategory?.Slug;

        var canonicalUrl =
            !string.IsNullOrWhiteSpace(citySlug) &&
            !string.IsNullOrWhiteSpace(categorySlug) &&
            !string.IsNullOrWhiteSpace(subCategorySlug)
                ? $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{entity.Slug}-{entity.Id}"
                : $"/{culture}/listing/{entity.Slug}-{entity.Id}";

        return new ListingDetailsPageVm
        {
            Culture = culture,
            Id = entity.Id,
            Title = entity.Title,
            Slug = entity.Slug,
            Url = canonicalUrl,

            ShortDescription = entity.ShortDescription,
            Description = entity.Description,

            CategoryName = entity.Category?.Name,
            CategorySlug = entity.Category?.Slug,
            CategoryUrl = entity.Category is null || entity.City is null
                ? null
                : $"/{culture}/{entity.City.Slug}/{entity.Category.Slug}",

            SubCategoryName = entity.SubCategory?.Name,
            SubCategorySlug = entity.SubCategory?.Slug,
            SubCategoryUrl = entity.SubCategory is null || entity.Category is null || entity.City is null
                ? null
                : $"/{culture}/{entity.City.Slug}/{entity.Category.Slug}/{entity.SubCategory.Slug}",

            CityName = entity.City?.Name,
            CitySlug = entity.City?.Slug,
            CityUrl = entity.City is null ? null : $"/{culture}/{entity.City.Slug}",

            Address = entity.Address,
            Rating = entity.Rating,
            ReviewsCount = entity.ReviewsCount,

            Latitude = entity.Latitude,
            Longitude = entity.Longitude,

            Contact = new ListingContactVm
            {
                ContactName = entity.Title,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website
            },

            Gallery = new ListingGalleryVm
            {
                Images = entity.Images
                    .OrderBy(x => x.SortOrder)
                    .Select(x => new ListingImageVm
                    {
                        Url = x.Url,
                        Alt = string.IsNullOrWhiteSpace(x.Alt) ? entity.Title : x.Alt!,
                        IsPrimary = x.IsPrimary,
                        SortOrder = x.SortOrder
                    })
                    .ToList()
            },

            Reviews = entity.Reviews
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(MapReview)
                .ToList(),

            RelatedListings = relatedListings ?? Array.Empty<RelatedListingVm>()
        };
    }

    public ListingReviewVm MapReview(ListingReview entity)
    {
        return new ListingReviewVm
        {
            AuthorName = entity.AuthorName,
            Text = entity.Text,
            Rating = entity.Rating,
            CreatedAtUtc = entity.CreatedAtUtc
        };
    }

    public RelatedListingVm MapRelatedListing(Listing entity, string culture)
    {
        var citySlug = entity.City?.Slug ?? string.Empty;
        var categorySlug = entity.Category?.Slug ?? string.Empty;
        var subCategorySlug = entity.SubCategory?.Slug ?? string.Empty;

        return new RelatedListingVm
        {
            Id = entity.Id,
            Title = entity.Title,
            Url = $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{entity.Slug}-{entity.Id}",
            ImageUrl = entity.Images
                .OrderBy(x => x.SortOrder)
                .FirstOrDefault(x => x.IsPrimary)?.Url
                ?? entity.Images.OrderBy(x => x.SortOrder).FirstOrDefault()?.Url,
            CityName = entity.City?.Name,
            SubCategoryName = entity.SubCategory?.Name,
            Rating = entity.Rating
        };
    }
}