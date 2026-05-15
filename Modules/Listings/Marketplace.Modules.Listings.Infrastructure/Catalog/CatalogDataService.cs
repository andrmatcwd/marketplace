using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using Marketplace.Modules.Listings.Application.Reviews.Filters;
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

    public async Task<IReadOnlyList<ListingMapMarkerDto>> GetListingMapMarkersAsync(
        int cityId, int? categoryId, int? subCategoryId, CancellationToken cancellationToken)
    {
        var query = _db.Listings
            .AsNoTracking()
            .Where(x => x.Status == ListingStatus.Active &&
                        x.CityId == cityId &&
                        x.Latitude != null &&
                        x.Longitude != null);

        if (categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);

        if (subCategoryId.HasValue)
            query = query.Where(x => x.SubCategoryId == subCategoryId.Value);

        return await query
            .Select(x => new ListingMapMarkerDto(
                x.Id,
                x.Title,
                x.Slug,
                x.Latitude!.Value,
                x.Longitude!.Value,
                x.City.Slug,
                x.Category.Slug,
                x.SubCategory.Slug))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PublicVacancyDto>> GetPublicVacanciesAsync(
        VacancyListingFilter filter, CancellationToken cancellationToken)
    {
        var query = _db.ListingVacancies
            .AsNoTracking()
            .Where(v => v.Listing.Status == ListingStatus.Active);

        if (filter.CityId.HasValue)
            query = query.Where(v => v.Listing.CityId == filter.CityId.Value);

        if (!string.IsNullOrWhiteSpace(filter.EmploymentType))
            query = query.Where(v => v.EmploymentType == filter.EmploymentType);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.ToLower();
            query = query.Where(v =>
                EF.Functions.Like(v.Title.ToLower(), $"%{s}%") ||
                (v.Description != null && EF.Functions.Like(v.Description.ToLower(), $"%{s}%")));
        }

        return await query
            .OrderByDescending(v => v.Id)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(v => new PublicVacancyDto(
                v.Id,
                v.Title,
                v.Description,
                v.EmploymentType,
                v.SalaryText,
                v.LocationText,
                v.ListingId,
                v.Listing.Title,
                v.Listing.Slug,
                v.Listing.City.Name,
                v.Listing.City.Slug,
                v.Listing.Category.Name,
                v.Listing.Category.Slug,
                v.Listing.SubCategory.Name,
                v.Listing.SubCategory.Slug))
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountPublicVacanciesAsync(
        VacancyListingFilter filter, CancellationToken cancellationToken)
    {
        var query = _db.ListingVacancies
            .AsNoTracking()
            .Where(v => v.Listing.Status == ListingStatus.Active);

        if (filter.CityId.HasValue)
            query = query.Where(v => v.Listing.CityId == filter.CityId.Value);

        if (!string.IsNullOrWhiteSpace(filter.EmploymentType))
            query = query.Where(v => v.EmploymentType == filter.EmploymentType);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.ToLower();
            query = query.Where(v =>
                EF.Functions.Like(v.Title.ToLower(), $"%{s}%") ||
                (v.Description != null && EF.Functions.Like(v.Description.ToLower(), $"%{s}%")));
        }

        return query.CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> GetDistinctVacancyEmploymentTypesAsync(
        CancellationToken cancellationToken)
    {
        return await _db.ListingVacancies
            .AsNoTracking()
            .Where(v => v.EmploymentType != null && v.Listing.Status == ListingStatus.Active)
            .Select(v => v.EmploymentType!)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync(cancellationToken);
    }

    public async Task SubmitPublicReviewAsync(
        int listingId, string userId, string authorName, string text, int rating,
        CancellationToken cancellationToken)
    {
        var listingExists = await _db.Listings
            .AnyAsync(x => x.Id == listingId && x.Status == ListingStatus.Active, cancellationToken);

        if (!listingExists) return;

        var reviewer = await _db.Reviewers
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (reviewer is null)
        {
            reviewer = new Reviewer
            {
                UserId = userId,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };
            _db.Reviewers.Add(reviewer);
            await _db.SaveChangesAsync(cancellationToken);
        }

        _db.Reviews.Add(new Review
        {
            ListingId = listingId,
            ReviewerId = reviewer.Id,
            AuthorName = authorName,
            Text = text,
            Rating = rating,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync(cancellationToken);

        var stats = await _db.Reviews
            .Where(r => r.ListingId == listingId)
            .GroupBy(r => r.ListingId)
            .Select(g => new { Count = g.Count(), Avg = g.Average(r => r.Rating) })
            .FirstOrDefaultAsync(cancellationToken);

        if (stats is not null)
        {
            var listing = await _db.Listings.FindAsync([listingId], cancellationToken);
            if (listing is not null)
            {
                listing.ReviewsCount = stats.Count;
                listing.Rating = Math.Round(stats.Avg, 1);
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task<PagedResult<ReviewDto>> GetAdminReviewsAsync(
        ReviewFilter filter, CancellationToken cancellationToken)
    {
        IQueryable<Review> query = _db.Reviews.AsNoTracking().Include(r => r.Listing);

        if (filter.ListingId.HasValue)
            query = query.Where(r => r.ListingId == filter.ListingId.Value);

        if (filter.Rating.HasValue)
            query = query.Where(r => (int)r.Rating == filter.Rating.Value);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(r => r.CreatedAtUtc)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                ListingId = r.ListingId,
                ListingTitle = r.Listing.Title,
                AuthorName = r.AuthorName,
                Text = r.Text,
                Rating = r.Rating,
                CreatedAtUtc = r.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<ReviewDto>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = total
        };
    }

    public Task<ReviewDto?> GetAdminReviewByIdAsync(int id, CancellationToken cancellationToken)
        => _db.Reviews
            .AsNoTracking()
            .Include(r => r.Listing)
            .Where(r => r.Id == id)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                ListingId = r.ListingId,
                ListingTitle = r.Listing.Title,
                AuthorName = r.AuthorName,
                Text = r.Text,
                Rating = r.Rating,
                CreatedAtUtc = r.CreatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task DeleteReviewAndRecalculateAsync(int id, CancellationToken cancellationToken)
    {
        var review = await _db.Reviews.FindAsync([id], cancellationToken);
        if (review is null) return;

        var listingId = review.ListingId;
        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync(cancellationToken);

        var stats = await _db.Reviews
            .Where(r => r.ListingId == listingId)
            .GroupBy(r => r.ListingId)
            .Select(g => new { Count = g.Count(), Avg = g.Average(r => r.Rating) })
            .FirstOrDefaultAsync(cancellationToken);

        var listing = await _db.Listings.FindAsync([listingId], cancellationToken);
        if (listing is not null)
        {
            listing.ReviewsCount = stats?.Count ?? 0;
            listing.Rating = stats is not null ? Math.Round(stats.Avg, 1) : 0;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

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
