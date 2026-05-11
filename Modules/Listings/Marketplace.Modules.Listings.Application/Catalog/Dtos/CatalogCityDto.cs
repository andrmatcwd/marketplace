namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record CatalogCityDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int SortOrder,
    int ListingsCount);
