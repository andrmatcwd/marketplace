namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record ListingImageDto(
    string Url,
    string? Alt,
    bool IsPrimary,
    int SortOrder);
