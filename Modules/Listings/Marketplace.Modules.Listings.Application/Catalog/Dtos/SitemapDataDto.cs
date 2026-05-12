namespace Marketplace.Modules.Listings.Application.Catalog.Dtos;

public sealed record SitemapDataDto(
    IReadOnlyList<SitemapCityDto> Cities,
    IReadOnlyList<SitemapCategoryDto> Categories,
    IReadOnlyList<SitemapSubCategoryDto> SubCategories,
    IReadOnlyList<SitemapListingDto> Listings);

public sealed record SitemapCityDto(string Slug);
public sealed record SitemapCategoryDto(string Slug);
public sealed record SitemapSubCategoryDto(string Slug, string CategorySlug);
public sealed record SitemapListingDto(int Id, string Slug, string CitySlug, string CategorySlug, string SubCategorySlug, DateTime UpdatedAtUtc);
