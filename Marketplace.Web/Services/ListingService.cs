using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services;

public class ListingService : IListingService
{
    private readonly ISender _sender;

    public ListingService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<ListingDetailsPageVm?> GetListingDetailsPageAsync(
        string city,
        string categorySlug,
        string subCategorySlug,
        string listingSlug,
        int listingId,
        CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(listingId), cancellationToken);

        if (listing is null)
            return null;

        // var relatedListings = await _db.Listings
        //     .AsNoTracking()
        //     .Include(x => x.Category)
        //     .Include(x => x.SubCategory)
        //     .Include(x => x.City)
        //     .Where(x =>
        //         x.Id != listing.Id &&
        //         x.Status == ListingStatus.Active &&
        //         x.CityId == listing.CityId &&
        //         x.CategoryId == listing.CategoryId)
        //     //.OrderByDescending(x => x.RatingAverage)
        //     //.ThenByDescending(x => x.ReviewsCount)
        //     .OrderBy(x => x.CreatedAtUtc)
        //     .Take(8)
        //     .Select(x => new ListingCardVm
        //     {
        //         Id = x.Id,
        //         Title = x.Title,
        //         Slug = x.Slug,
        //         SubcategoryName = x.SubCategory.Name,
        //         SubcategorySlug = x.SubCategory.Slug,
        //         //Rating = x.ReviewsCount > 0 ? x.RatingAverage : 0,
        //         //ReviewsCount = x.ReviewsCount,
        //         Url = BuildListingUrl(
        //             x.City.Slug,
        //             x.Category.Slug,
        //             x.SubCategory.Slug,
        //             x.Slug,
        //             x.Id)
        //     })
        //     .ToListAsync(cancellationToken);

        // var mainImage = listing.Images
        //     .OrderByDescending(x => x.IsPrimary)
        //     .ThenBy(x => x.Id)
        //     .FirstOrDefault();

        // var images = listing.Images
        //     .OrderByDescending(x => x.IsPrimary)
        //     .ThenBy(x => x.Id)
        //     .Select((x, index) => new ListingImageVm
        //     {
        //         Url = x.Url,
        //         Alt = x.AltText,
        //         SortOrder = index
        //     })
        //     .ToList();

        // var reviews = listing.Reviews
        //     .OrderByDescending(x => x.CreatedAtUtc)
        //     .Take(10)
        //     .Select(x => new ListingReviewVm
        //     {
        //         // AuthorName = x.Reviewer != null
        //         //     ? $"{x.Reviewer.FirstName} {x.Reviewer.LastName}".Trim()
        //         //     : "Користувач",
        //         AuthorName = "Користувач",
        //         Rating = x.Rating,
        //         Text = string.IsNullOrWhiteSpace(x.Comment) ? null : x.Comment,
        //         CreatedAtUtc = x.CreatedAtUtc
        //     })
        //     .ToList();

        // var features = BuildServiceFeatures(listing);

        return new ListingDetailsPageVm
        {
            Id = listing.Id,

            CityName = listing.CityName,
            CitySlug = listing.CitySlug,

            CategoryName = listing.CategoryName,
            CategorySlug = listing.CategorySlug,

            SubCategoryName = listing.SubCategoryName,
            SubCategorySlug = listing.SubCategorySlug,

            ListingSlug = listing.Slug,

            Title = listing.Title,
            H1 = listing.Title,
            ShortDescription = BuildShortDescription(listing.Description),
            Description = listing.Description,

            ProviderName = null,
            Phone = null,
            Email = null,
            Website = null,
            Address = listing.AddressLine,

            CityDistrict = null,
            Latitude = listing.Latitude.HasValue ? (decimal?)listing.Latitude.Value : null,
            Longitude = listing.Longitude.HasValue ? (decimal?)listing.Longitude.Value : null,

            //Rating = listing.ReviewsCount > 0 ? (decimal?)listing.RatingAverage : null,
            //ReviewsCount = listing.ReviewsCount,
            IsVerified = listing.SubscriptionType != SubscriptionType.Free,

            // MainImageUrl = mainImage?.Url,
            // Images = images,

            //PriceText = BuildPriceText(listing),
            //ServiceFeatures = features,

            // Reviews = reviews,
            // RelatedListings = relatedListings,

            Breadcrumbs = new[]
            {
                new BreadcrumbItemVm
                {
                    Title = "Каталог",
                    Url = "/listings",
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = listing.CityName,
                    Url = "/" + listing.CitySlug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = listing.CategoryName,
                    Url = "/" + listing.CitySlug + "/" + listing.CategorySlug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = listing.SubCategoryName,
                    Url = "/" + listing.CitySlug + "/" + listing.CategorySlug + "/" + listing.SubCategorySlug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = listing.Title,
                    Url = BuildListingUrl(
                        listing.CitySlug,
                        listing.CategorySlug,
                        listing.SubCategorySlug,
                        listing.Slug,
                        listing.Id),
                    IsCurrent = true
                }
            }
        };
    }

    private static string BuildListingUrl(
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string listingSlug,
        int listingId)
    {
        return $"/{citySlug}/{categorySlug}/{subCategorySlug}/{listingSlug}-{listingId}";
    }

    private static string? BuildShortDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var text = description.Trim();

        if (text.Length <= 180)
            return text;

        return text[..180].TrimEnd() + "...";
    }

    // private static string? BuildPriceText(Listing listing)
    // {
    //     if (listing.Price <= 0)
    //         return null;

    //     return $"{listing.Price:0.##} {listing.Currency}";
    // }

    private static IReadOnlyCollection<string> BuildServiceFeatures(Listing listing)
    {
        var items = new List<string>();

        if (!string.IsNullOrWhiteSpace(listing.Category?.Name))
            items.Add($"Категорія: {listing.Category.Name}");

        if (!string.IsNullOrWhiteSpace(listing.SubCategory?.Name))
            items.Add($"Підкатегорія: {listing.SubCategory.Name}");

        if (!string.IsNullOrWhiteSpace(listing.City?.Name))
            items.Add($"Місто: {listing.City.Name}");

        if (!string.IsNullOrWhiteSpace(listing.AddressLine))
            items.Add("Є адреса");

        if (listing.Latitude.HasValue && listing.Longitude.HasValue)
            items.Add("Є координати");

        // if (listing.ReviewsCount > 0)
        //     items.Add($"{listing.ReviewsCount} відгуків");

        if (listing.SubscriptionType != SubscriptionType.Free)
            items.Add("Преміум розміщення");

        return items;
    }
}