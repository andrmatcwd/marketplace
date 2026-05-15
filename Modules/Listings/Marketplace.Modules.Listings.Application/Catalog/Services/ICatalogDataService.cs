using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using Marketplace.Modules.Listings.Application.Reviews.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Catalog.Services;

public interface ICatalogDataService
{
    // Cities
    Task<IReadOnlyList<(City City, int ListingsCount)>> GetPublishedCitiesAsync(
        CancellationToken cancellationToken, int? take = null);

    Task<City?> GetPublishedCityBySlugAsync(string slug, CancellationToken cancellationToken);

    // Categories
    Task<IReadOnlyList<(Category Category, int ListingsCount)>> GetPublishedCategoriesAsync(
        CancellationToken cancellationToken, int? take = null);

    Task<IReadOnlyList<(Category Category, int ListingsCount)>> GetCityCategoriesAsync(
        int cityId, CancellationToken cancellationToken);

    Task<Category?> GetPublishedCategoryBySlugAsync(string slug, CancellationToken cancellationToken);

    // SubCategories
    Task<IReadOnlyList<(SubCategory SubCategory, int ListingsCount)>> GetPopularCitySubCategoriesAsync(
        int cityId, CancellationToken cancellationToken, int take = 12);

    Task<IReadOnlyList<(SubCategory SubCategory, int ListingsCount)>> GetCategorySubCategoriesInCityAsync(
        int cityId, int categoryId, CancellationToken cancellationToken);

    Task<SubCategory?> GetPublishedSubCategoryWithCategoryAsync(
        string categorySlug, string subCategorySlug, CancellationToken cancellationToken);

    // Listings
    Task<IReadOnlyList<Listing>> GetPublishedListingsAsync(
        CatalogListingFilter filter, CancellationToken cancellationToken);

    Task<int> CountPublishedListingsAsync(
        CatalogListingFilter filter, CancellationToken cancellationToken);

    Task<IReadOnlyList<Listing>> GetFeaturedListingsAsync(int take, CancellationToken cancellationToken);

    Task<Listing?> GetPublishedListingByIdAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Listing>> GetRelatedListingsAsync(
        int excludeId, int cityId, int subCategoryId, int take, CancellationToken cancellationToken);

    Task<IReadOnlyList<Listing>> GetTopListingsByCategoryInCityAsync(
        int cityId, int categoryId, string? search, int take, CancellationToken cancellationToken);

    Task<int> CountListingsByCategoryInCityAsync(
        int cityId, int categoryId, string? search, CancellationToken cancellationToken);

    Task<IReadOnlyList<ListingMapMarkerDto>> GetListingMapMarkersAsync(
        int cityId, int? categoryId, int? subCategoryId, CancellationToken cancellationToken);

    // Vacancies
    Task<IReadOnlyList<PublicVacancyDto>> GetPublicVacanciesAsync(
        VacancyListingFilter filter, CancellationToken cancellationToken);

    Task<int> CountPublicVacanciesAsync(
        VacancyListingFilter filter, CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetDistinctVacancyEmploymentTypesAsync(CancellationToken cancellationToken);

    // Reviews
    Task SubmitPublicReviewAsync(
        int listingId, string userId, string authorName, string text, int rating,
        CancellationToken cancellationToken);

    Task<PagedResult<ReviewDto>> GetAdminReviewsAsync(ReviewFilter filter, CancellationToken cancellationToken);
    Task<ReviewDto?> GetAdminReviewByIdAsync(int id, CancellationToken cancellationToken);
    Task DeleteReviewAndRecalculateAsync(int id, CancellationToken cancellationToken);

    // Search / AutoComplete helpers
    Task<int?> GetCityIdBySlugAsync(string citySlug, CancellationToken cancellationToken);

    Task<IReadOnlyList<SubCategorySearchResult>> SearchSubCategoriesInCityAsync(
        int cityId, string search, int take, CancellationToken cancellationToken);

    Task<SitemapDataDto> GetSitemapDataAsync(CancellationToken cancellationToken);
}

public sealed record SubCategorySearchResult(
    string Name,
    string Slug,
    string CategoryName,
    string CategorySlug,
    int ListingsCount,
    int Score);
