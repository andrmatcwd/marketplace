using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Catalog;

namespace Marketplace.Web.Services.Catalog;

public interface ICatalogLookupService
{
    Task<IReadOnlyCollection<CityLookupItem>> GetPublishedCitiesAsync(
        CancellationToken cancellationToken,
        int? take = null);

    Task<IReadOnlyCollection<CategoryLookupItem>> GetPublishedCategoriesAsync(
        CancellationToken cancellationToken,
        int? take = null);

    Task<IReadOnlyCollection<CategoryLookupItem>> GetCityCategoriesAsync(
        Guid cityId,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<SubCategoryLookupItem>> GetPopularCitySubCategoriesAsync(
        Guid cityId,
        CancellationToken cancellationToken,
        int take = 12);

    Task<IReadOnlyCollection<SubCategoryLookupItem>> GetCategorySubCategoriesInCityAsync(
        Guid cityId,
        Guid categoryId,
        CancellationToken cancellationToken);

    Task<City?> GetPublishedCityBySlugAsync(
        string citySlug,
        CancellationToken cancellationToken);

    Task<Category?> GetPublishedCategoryBySlugAsync(
        string categorySlug,
        CancellationToken cancellationToken);

    Task<SubCategory?> GetPublishedSubCategoryWithCategoryAsync(
        string categorySlug,
        string subCategorySlug,
        CancellationToken cancellationToken);
}