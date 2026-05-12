using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Modules.Listings.Infrastructure.Catalog;

public sealed class CatalogDataService : ICatalogDataService
{
    private readonly ListingsDbContext _db;

    public CatalogDataService(ListingsDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<(City City, int ListingsCount)>> GetPublishedCitiesAsync(
        CancellationToken cancellationToken, int? take = null)
    {
        var query = _db.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                City = x,
                ListingsCount = _db.Listings.Count(l => l.CityId == x.Id && l.Status == ListingStatus.Active)
            });

        if (take.HasValue) query = query.Take(take.Value);

        var rows = await query.ToListAsync(cancellationToken);
        return rows.Select(r => (r.City, r.ListingsCount)).ToList();
    }

    public Task<City?> GetPublishedCityBySlugAsync(string slug, CancellationToken cancellationToken)
        => _db.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == slug, cancellationToken);

    public async Task<IReadOnlyList<(Category Category, int ListingsCount)>> GetPublishedCategoriesAsync(
        CancellationToken cancellationToken, int? take = null)
    {
        var query = _db.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Category = x,
                ListingsCount = _db.Listings.Count(l => l.CategoryId == x.Id && l.Status == ListingStatus.Active)
            });

        if (take.HasValue) query = query.Take(take.Value);

        var rows = await query.ToListAsync(cancellationToken);
        return rows.Select(r => (r.Category, r.ListingsCount)).ToList();
    }

    public async Task<IReadOnlyList<(Category Category, int ListingsCount)>> GetCityCategoriesAsync(
        int cityId, CancellationToken cancellationToken)
    {
        var rows = await _db.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Listings.Any(l => l.CityId == cityId && l.Status == ListingStatus.Active))
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Category = x,
                ListingsCount = x.Listings.Count(l => l.CityId == cityId && l.Status == ListingStatus.Active)
            })
            .ToListAsync(cancellationToken);

        return rows.Select(r => (r.Category, r.ListingsCount)).ToList();
    }

    public Task<Category?> GetPublishedCategoryBySlugAsync(string slug, CancellationToken cancellationToken)
        => _db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == slug, cancellationToken);

    public async Task<IReadOnlyList<(SubCategory SubCategory, int ListingsCount)>> GetPopularCitySubCategoriesAsync(
        int cityId, CancellationToken cancellationToken, int take = 12)
    {
        var rows = await _db.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x =>
                x.IsPublished &&
                x.Category != null &&
                x.Category.IsPublished &&
                x.Listings.Any(l => l.CityId == cityId && l.Status == ListingStatus.Active))
            .OrderByDescending(x => x.Listings.Count(l => l.CityId == cityId && l.Status == ListingStatus.Active))
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                SubCategory = x,
                ListingsCount = x.Listings.Count(l => l.CityId == cityId && l.Status == ListingStatus.Active)
            })
            .Take(take)
            .ToListAsync(cancellationToken);

        return rows.Select(r => (r.SubCategory, r.ListingsCount)).ToList();
    }

    public async Task<IReadOnlyList<(SubCategory SubCategory, int ListingsCount)>> GetCategorySubCategoriesInCityAsync(
        int cityId, int categoryId, CancellationToken cancellationToken)
    {
        var rows = await _db.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.IsPublished && x.CategoryId == categoryId)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                SubCategory = x,
                ListingsCount = x.Listings.Count(l => l.Status == ListingStatus.Active && l.CityId == cityId)
            })
            .ToListAsync(cancellationToken);

        return rows.Select(r => (r.SubCategory, r.ListingsCount)).ToList();
    }

    public Task<SubCategory?> GetPublishedSubCategoryWithCategoryAsync(
        string categorySlug, string subCategorySlug, CancellationToken cancellationToken)
        => _db.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.IsPublished &&
                     x.Slug == subCategorySlug &&
                     x.Category != null &&
                     x.Category.Slug == categorySlug,
                cancellationToken);

    public async Task<IReadOnlyList<Listing>> GetPublishedListingsAsync(
        CatalogListingFilter filter, CancellationToken cancellationToken)
        => await _db.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .ApplyFilter(filter)
            .ApplySorting(filter.Sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

    public Task<int> CountPublishedListingsAsync(
        CatalogListingFilter filter, CancellationToken cancellationToken)
        => _db.Listings
            .AsNoTracking()
            .Published()
            .ApplyFilter(filter)
            .CountAsync(cancellationToken);

    public async Task<IReadOnlyList<Listing>> GetFeaturedListingsAsync(int take, CancellationToken cancellationToken)
        => await _db.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .ApplySorting("rating")
            .Take(take)
            .ToListAsync(cancellationToken);

    public Task<Listing?> GetPublishedListingByIdAsync(int id, CancellationToken cancellationToken)
        => _db.Listings
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.City)
            .Include(x => x.Images)
            .Include(x => x.Reviews).ThenInclude(r => r.Reviewer)
            .Include(x => x.Rental).ThenInclude(r => r.RoomOptions)
            .Include(x => x.Vacancies)
            .FirstOrDefaultAsync(x => x.Status == ListingStatus.Active && x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Listing>> GetRelatedListingsAsync(
        int excludeId, int cityId, int subCategoryId, int take, CancellationToken cancellationToken)
        => await _db.Listings
            .AsNoTracking()
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Include(x => x.Images)
            .Where(x =>
                x.Status == ListingStatus.Active &&
                x.Id != excludeId &&
                x.CityId == cityId &&
                x.SubCategoryId == subCategoryId)
            .OrderByDescending(x => x.Rating)
            .ThenByDescending(x => x.ReviewsCount)
            .Take(take)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Listing>> GetTopListingsByCategoryInCityAsync(
        int cityId, int categoryId, string? search, int take, CancellationToken cancellationToken)
        => await _db.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .Where(x => x.CityId == cityId && x.CategoryId == categoryId)
            .ApplySearch(search)
            .ApplySorting("rating")
            .Take(take)
            .ToListAsync(cancellationToken);

    public Task<int> CountListingsByCategoryInCityAsync(
        int cityId, int categoryId, string? search, CancellationToken cancellationToken)
        => _db.Listings
            .AsNoTracking()
            .Published()
            .Where(x => x.CityId == cityId && x.CategoryId == categoryId)
            .ApplySearch(search)
            .CountAsync(cancellationToken);

    public Task<int?> GetCityIdBySlugAsync(string citySlug, CancellationToken cancellationToken)
        => _db.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Slug == citySlug)
            .Select(x => (int?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<SitemapDataDto> GetSitemapDataAsync(CancellationToken cancellationToken)
    {
        var cities = await _db.Cities.AsNoTracking()
            .Where(x => x.IsPublished)
            .Select(x => new SitemapCityDto(x.Slug))
            .ToListAsync(cancellationToken);

        var categories = await _db.Categories.AsNoTracking()
            .Where(x => x.IsPublished)
            .Select(x => new SitemapCategoryDto(x.Slug))
            .ToListAsync(cancellationToken);

        var subCategories = await _db.SubCategories.AsNoTracking()
            .Where(x => x.IsPublished && x.Category.IsPublished)
            .Select(x => new SitemapSubCategoryDto(x.Slug, x.Category.Slug))
            .ToListAsync(cancellationToken);

        var listings = await _db.Listings.AsNoTracking()
            .Where(x => x.Status == ListingStatus.Active)
            .Select(x => new SitemapListingDto(x.Id, x.Slug, x.City.Slug, x.Category.Slug, x.SubCategory.Slug, x.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

        return new SitemapDataDto(cities, categories, subCategories, listings);
    }

    public async Task<IReadOnlyList<SubCategorySearchResult>> SearchSubCategoriesInCityAsync(
        int cityId, string search, int take, CancellationToken cancellationToken)
    {
        var normalized = search.ToLower();

        return await _db.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x =>
                x.IsPublished &&
                x.Category != null &&
                x.Category.IsPublished &&
                x.Listings.Any(l => l.CityId == cityId && l.Status == ListingStatus.Active) &&
                (EF.Functions.Like(x.Name.ToLower(), $"%{normalized}%") ||
                 EF.Functions.Like(x.Slug.ToLower(), $"%{normalized}%")))
            .Select(x => new SubCategorySearchResult(
                x.Name,
                x.Slug,
                x.Category!.Name,
                x.Category!.Slug,
                x.Listings.Count(l => l.CityId == cityId && l.Status == ListingStatus.Active),
                x.Name.ToLower() == normalized || x.Slug.ToLower() == normalized ? 300 :
                x.Name.ToLower().StartsWith(normalized) || x.Slug.ToLower().StartsWith(normalized) ? 200 :
                100))
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
