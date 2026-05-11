namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingCardDto(
    int Id,
    string Title,
    string Slug,
    string? ShortDescription,
    double Rating,
    int ReviewsCount,
    string CityName,
    string CitySlug,
    string CategoryName,
    string CategorySlug,
    string SubCategoryName,
    string SubCategorySlug,
    string? PrimaryImageUrl,
    string? PrimaryImageAlt);
