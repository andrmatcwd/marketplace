namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record CatalogSubCategoryDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? Icon,
    int SortOrder,
    int CategoryId,
    string CategoryName,
    string CategorySlug,
    int ListingsCount);
