namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed class CatalogListingFilter
{
    public string? Search { get; init; }
    public string? CitySlug { get; init; }
    public int? CityId { get; init; }
    public string? CategorySlug { get; init; }
    public int? SubCategoryId { get; init; }
    public string? Sort { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
}
