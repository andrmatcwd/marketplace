namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingMapMarkerDto(
    int Id,
    string Title,
    string Slug,
    double Latitude,
    double Longitude,
    string CitySlug,
    string CategorySlug,
    string SubCategorySlug);
