using Marketplace.Web.Data;
using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogLookupService : ICatalogLookupService
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogLookupService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<CityLookupItem>> GetPublishedCitiesAsync(
        CancellationToken cancellationToken,
        int? take = null)
    {
        var query = _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new CityLookupItem
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CityId == x.Id && l.IsPublished)
            });

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CategoryLookupItem>> GetPublishedCategoriesAsync(
        CancellationToken cancellationToken,
        int? take = null)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new CategoryLookupItem
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CategoryId == x.Id && l.IsPublished)
            });

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CategoryLookupItem>> GetCityCategoriesAsync(
        Guid cityId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Listings.Any(l => l.CityId == cityId && l.IsPublished))
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new CategoryLookupItem
            {
                Entity = x,
                ListingsCount = x.Listings.Count(l => l.CityId == cityId && l.IsPublished)
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<SubCategoryLookupItem>> GetPopularCitySubCategoriesAsync(
        Guid cityId,
        CancellationToken cancellationToken,
        int take = 12)
    {
        return await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x =>
                x.IsPublished &&
                x.Category != null &&
                x.Category.IsPublished &&
                x.Listings.Any(l => l.CityId == cityId && l.IsPublished))
            .OrderByDescending(x => x.Listings.Count(l => l.CityId == cityId && l.IsPublished))
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new SubCategoryLookupItem
            {
                Entity = x,
                ListingsCount = x.Listings.Count(l => l.CityId == cityId && l.IsPublished)
            })
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<SubCategoryLookupItem>> GetCategorySubCategoriesInCityAsync(
        Guid cityId,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.IsPublished && x.CategoryId == categoryId)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new SubCategoryLookupItem
            {
                Entity = x,
                ListingsCount = x.Listings.Count(l => l.IsPublished && l.CityId == cityId)
            })
            .ToListAsync(cancellationToken);
    }

    public Task<City?> GetPublishedCityBySlugAsync(
        string citySlug,
        CancellationToken cancellationToken)
    {
        return _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == citySlug, cancellationToken);
    }

    public Task<Category?> GetPublishedCategoryBySlugAsync(
        string categorySlug,
        CancellationToken cancellationToken)
    {
        return _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == categorySlug, cancellationToken);
    }

    public Task<SubCategory?> GetPublishedSubCategoryWithCategoryAsync(
        string categorySlug,
        string subCategorySlug,
        CancellationToken cancellationToken)
    {
        return _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.IsPublished &&
                     x.Slug == subCategorySlug &&
                     x.Category != null &&
                     x.Category.Slug == categorySlug,
                cancellationToken);
    }
}