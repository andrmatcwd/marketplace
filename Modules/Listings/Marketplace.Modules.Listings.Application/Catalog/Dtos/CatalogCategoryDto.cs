namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record CatalogCategoryDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? Icon,
    int SortOrder,
    int ListingsCount);
