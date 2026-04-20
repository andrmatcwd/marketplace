using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.Listings.Forms;

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

            RelatedListings = relatedListings ?? Array.Empty<RelatedListingVm>(),
            ReviewForm = new CreateListingReviewVm
            {
                ListingId = entity.Id
            },

            ServiceFeatures = BuildServiceFeatures(entity),
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
            Url = $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{entity.Slug}/{entity.Id}",
            ImageUrl = entity.Images
                .OrderBy(x => x.SortOrder)
                .FirstOrDefault(x => x.IsPrimary)?.Url
                ?? entity.Images.OrderBy(x => x.SortOrder).FirstOrDefault()?.Url,
            CityName = entity.City?.Name,
            SubCategoryName = entity.SubCategory?.Name,
            Rating = entity.Rating
        };
    }

    private static IReadOnlyCollection<string> BuildServiceFeatures(Domain.Entities.Listing entity)
{
    var features = new List<string>();

    if (!string.IsNullOrWhiteSpace(entity.Phone))
    {
        features.Add("Є телефон для звʼязку");
    }

    if (!string.IsNullOrWhiteSpace(entity.Email))
    {
        features.Add("Є email для звернення");
    }

    if (!string.IsNullOrWhiteSpace(entity.Website))
    {
        features.Add("Є сайт або сторінка компанії");
    }

    if (!string.IsNullOrWhiteSpace(entity.Address))
    {
        features.Add("Є фізична адреса");
    }

    if (entity.Latitude.HasValue && entity.Longitude.HasValue)
    {
        features.Add("Є точка на карті");
    }

    if (entity.Images != null && entity.Images.Any())
    {
        features.Add("Доступна галерея фото");
    }

    if (entity.Rating >= 4.5)
    {
        features.Add("Високий рейтинг");
    }

    if (entity.ReviewsCount >= 10)
    {
        features.Add("Достатньо відгуків для оцінки");
    }

    return features;
}
}