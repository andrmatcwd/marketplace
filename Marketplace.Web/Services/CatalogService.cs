using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.SubCategory;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services;

public class CatalogService : ICatalogService
{
    private readonly ListingsDbContext _db;

    public CatalogService(ListingsDbContext db)
    {
        _db = db;
    }

    public async Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(
        string culture,
        CancellationToken cancellationToken)
    {
        var cities = await _db.Cities
            .AsNoTracking()
            //.Where(x => x.Type.Equals(LocationType.City))
            .Select(x => new CityItemVm
            {
                Name = x.Name,
                Slug = x.Slug,
                ListingsCount = x.Listings.Count(l => l.Status == ListingStatus.Active),
                Url = "/" + x.Slug
            })
            .Where(x => x.ListingsCount > 0)
            .OrderByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .Take(24)
            .ToListAsync(cancellationToken);

        var popularCategories = await _db.Categories
            .AsNoTracking()
            .Select(x => new CategoryItemVm
            {
                Name = x.Name,
                Slug = x.Slug,
                ListingsCount = x.Listings.Count(l => l.Status == ListingStatus.Active),
                Url = "/" + x.Slug
            })
            .Where(x => x.ListingsCount > 0)
            .OrderByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .Take(12)
            .ToListAsync(cancellationToken);

        return new CatalogIndexPageVm
        {
            H1 = "Каталог послуг",
            IntroText = "Знайдіть послуги за містом, категорією та підкатегорією.",
            Cities = cities,
            PopularCategories = popularCategories,
            Filter = new BaseFilter(),
            Pagination = new PaginationVm
            {
                CurrentPage = 1,
                TotalPages = 1,
                PageSize = cities.Count,
                TotalItems = cities.Count
            },
            Breadcrumbs = new[]
            {
                new BreadcrumbItemVm
                {
                    Title = "Каталог",
                    Url = "/listings",
                    IsCurrent = true
                }
            }
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string city,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        filter = NormalizeFilter(filter);

        var cityEntity = await _db.Cities
            .AsNoTracking()
            .Where(x => x.Slug == city)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Slug
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (cityEntity is null)
            return null;

        var baseListings = _db.Listings
            .AsNoTracking()
            .Where(x =>
                x.Status == ListingStatus.Active &&
                x.CityId == cityEntity.Id);

        baseListings = ApplyListingFilters(baseListings, filter);

        var totalListingsCount = await baseListings.CountAsync(cancellationToken);

        var categories = await _db.Categories
            .AsNoTracking()
            .Select(x => new CategoryItemVm
            {
                Name = x.Name,
                Slug = x.Slug,
                ListingsCount = x.Listings.Count(l =>
                    l.Status == ListingStatus.Active &&
                    l.CityId == cityEntity.Id),
                Url = "/" + cityEntity.Slug + "/" + x.Slug
            })
            .Where(x => x.ListingsCount > 0)
            .OrderByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var featuredListings = await ApplyListingSort(baseListings, filter)
            .Take(12)
            .Select(x => new ListingCardVm
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                SubcategoryName = x.SubCategory.Name,
                SubcategorySlug = x.SubCategory.Slug,
                Rating = x.ReviewsCount > 0 ? x.RatingAverage : 0,
                ReviewsCount = x.ReviewsCount,
                Url = BuildListingUrl(
                    culture,
                    x.City.Slug,
                    x.Category.Slug,
                    x.SubCategory.Slug,
                    x.Slug,
                    x.Id)
            })
            .ToListAsync(cancellationToken);

        return new CityPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            H1 = $"Послуги у {cityEntity.Name}",
            IntroText = $"Перегляньте доступні категорії та популярні пропозиції у місті {cityEntity.Name}.",
            TotalListingsCount = totalListingsCount,
            Categories = categories,
            FeaturedListings = featuredListings,
            Filter = filter,
            Pagination = BuildPagination(1, 1, featuredListings.Count, featuredListings.Count),
            Breadcrumbs = new[]
            {
                new BreadcrumbItemVm
                {
                    Title = "Каталог",
                    Url = "/listings",
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = cityEntity.Name,
                    Url = "/" + cityEntity.Slug,
                    IsCurrent = true
                }
            }
        };
    }

    public async Task<CategoryPageVm?> GetCategoryPageAsync(
        string culture,
        string city,
        string categorySlug,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        filter = NormalizeFilter(filter);

        var cityEntity = await _db.Cities
            .AsNoTracking()
            .Where(x => x.Slug == city)
            .Select(x => new { x.Id, x.Name, x.Slug })
            .FirstOrDefaultAsync(cancellationToken);

        if (cityEntity is null)
            return null;

        var categoryEntity = await _db.Categories
            .AsNoTracking()
            .Where(x => x.Slug == categorySlug)
            .Select(x => new { x.Id, x.Name, x.Slug, x.Description })
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryEntity is null)
            return null;

        var baseListings = _db.Listings
            .AsNoTracking()
            .Where(x =>
                x.Status == ListingStatus.Active &&
                x.CityId == cityEntity.Id &&
                x.CategoryId == categoryEntity.Id);

        baseListings = ApplyListingFilters(baseListings, filter);

        var totalListingsCount = await baseListings.CountAsync(cancellationToken);

        var subcategories = await _db.SubCategories
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryEntity.Id)
            .Select(x => new SubCategoryItemVm
            {
                Name = x.Name,
                Slug = x.Slug,
                ListingsCount = x.Listings.Count(l =>
                    l.Status == ListingStatus.Active &&
                    l.CityId == cityEntity.Id),
                Url = "/" + cityEntity.Slug + "/" + categoryEntity.Slug + "/" + x.Slug
            })
            .Where(x => x.ListingsCount > 0)
            .OrderByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var page = filter.Page;
        var pageSize = filter.PageSize;

        var listings = await ApplyListingSort(baseListings, filter)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ListingCardVm
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                SubcategoryName = x.SubCategory.Name,
                SubcategorySlug = x.SubCategory.Slug,
                Rating = x.ReviewsCount > 0 ? x.RatingAverage : 0,
                ReviewsCount = x.ReviewsCount,
                Url = BuildListingUrl(
                    culture,
                    x.City.Slug,
                    x.Category.Slug,
                    x.SubCategory.Slug,
                    x.Slug,
                    x.Id)
            })
            .ToListAsync(cancellationToken);

        return new CategoryPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            CategoryName = categoryEntity.Name,
            CategorySlug = categoryEntity.Slug,
            H1 = $"{categoryEntity.Name} у {cityEntity.Name}",
            IntroText = categoryEntity.Description,
            TotalListingsCount = totalListingsCount,
            Subcategories = subcategories,
            Listings = listings,
            Filter = filter,
            Pagination = BuildPagination(page, pageSize, totalListingsCount, listings.Count),
            Breadcrumbs = new[]
            {
                new BreadcrumbItemVm
                {
                    Title = "Каталог",
                    Url = "/listings",
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = cityEntity.Name,
                    Url = "/" + cityEntity.Slug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = categoryEntity.Name,
                    Url = "/" + cityEntity.Slug + "/" + categoryEntity.Slug,
                    IsCurrent = true
                }
            }
        };
    }

    public async Task<SubCategoryPageVm?> GetSubCategoryPageAsync(
        string culture,
        string city,
        string categorySlug,
        string subCategorySlug,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        filter = NormalizeFilter(filter);

        var cityEntity = await _db.Cities
            .AsNoTracking()
            .Where(x => x.Slug == city)
            .Select(x => new { x.Id, x.Name, x.Slug })
            .FirstOrDefaultAsync(cancellationToken);

        if (cityEntity is null)
            return null;

        var categoryEntity = await _db.Categories
            .AsNoTracking()
            .Where(x => x.Slug == categorySlug)
            .Select(x => new { x.Id, x.Name, x.Slug })
            .FirstOrDefaultAsync(cancellationToken);

        if (categoryEntity is null)
            return null;

        var subCategoryEntity = await _db.SubCategories
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryEntity.Id && x.Slug == subCategorySlug)
            .Select(x => new { x.Id, x.Name, x.Slug, x.Description })
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategoryEntity is null)
            return null;

        var baseListings = _db.Listings
            .AsNoTracking()
            .Where(x =>
                x.Status == ListingStatus.Active &&
                x.CityId == cityEntity.Id &&
                x.CategoryId == categoryEntity.Id &&
                x.SubCategoryId == subCategoryEntity.Id);

        baseListings = ApplyListingFilters(baseListings, filter);

        var totalListingsCount = await baseListings.CountAsync(cancellationToken);

        var page = filter.Page;
        var pageSize = filter.PageSize;

        var listings = await ApplyListingSort(baseListings, filter)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ListingCardVm
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                SubcategoryName = x.SubCategory.Name,
                SubcategorySlug = x.SubCategory.Slug,
                Rating = x.ReviewsCount > 0 ? x.RatingAverage : 0,
                ReviewsCount = x.ReviewsCount,
                Url = BuildListingUrl(
                    culture,
                    x.City.Slug,
                    x.Category.Slug,
                    x.SubCategory.Slug,
                    x.Slug,
                    x.Id)
            })
            .ToListAsync(cancellationToken);

        return new SubCategoryPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            CategoryName = categoryEntity.Name,
            CategorySlug = categoryEntity.Slug,
            SubcategoryName = subCategoryEntity.Name,
            SubcategorySlug = subCategoryEntity.Slug,
            H1 = $"{subCategoryEntity.Name} у {cityEntity.Name}",
            IntroText = subCategoryEntity.Description,
            TotalListingsCount = totalListingsCount,
            Listings = listings,
            Filter = filter,
            Pagination = BuildPagination(page, pageSize, totalListingsCount, listings.Count),
            Breadcrumbs = new[]
            {
                new BreadcrumbItemVm
                {
                    Title = "Каталог",
                    Url = "/listings",
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = cityEntity.Name,
                    Url = "/" + cityEntity.Slug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = categoryEntity.Name,
                    Url = "/" + cityEntity.Slug + "/" + categoryEntity.Slug,
                    IsCurrent = false
                },
                new BreadcrumbItemVm
                {
                    Title = subCategoryEntity.Name,
                    Url = "/" + cityEntity.Slug + "/" + categoryEntity.Slug + "/" + subCategoryEntity.Slug,
                    IsCurrent = true
                }
            }
        };
    }

    private static BaseFilter NormalizeFilter(BaseFilter? filter)
    {
        filter ??= new BaseFilter();

        if (filter.Page < 1)
            filter.Page = 1;

        if (filter.PageSize <= 0)
            filter.PageSize = 20;

        if (filter.PageSize > 100)
            filter.PageSize = 100;

        if (string.IsNullOrWhiteSpace(filter.SortBy))
            filter.SortBy = "newest";

        return filter;
    }

    private static IQueryable<Listing> ApplyListingFilters(
        IQueryable<Listing> query,
        BaseFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var search = filter.SearchTerm.Trim();

            query = query.Where(x =>
                x.Title.Contains(search) ||
                x.Description.Contains(search));
        }

        if (filter.RatingFrom.HasValue)
        {
            query = query.Where(x => x.RatingAverage >= filter.RatingFrom.Value);
        }

        return query;
    }

    private static IQueryable<Listing> ApplyListingSort(
        IQueryable<Listing> query,
        BaseFilter filter)
    {
        return filter.SortBy?.ToLowerInvariant() switch
        {
            "rating" => query
                .OrderByDescending(x => x.RatingAverage)
                .ThenByDescending(x => x.ReviewsCount)
                .ThenBy(x => x.Title),

            "popular" => query
                .OrderByDescending(x => x.ReviewsCount)
                .ThenByDescending(x => x.RatingAverage)
                .ThenBy(x => x.Title),

            _ => query
                .OrderByDescending(x => x.CreatedAtUtc)
                .ThenByDescending(x => x.Id)
        };
    }

    private static PaginationVm BuildPagination(
        int currentPage,
        int pageSize,
        int totalItems,
        int currentItemsCount)
    {
        var totalPages = totalItems == 0
            ? 1
            : (int)Math.Ceiling(totalItems / (double)pageSize);

        if (currentPage > totalPages)
            currentPage = totalPages;

        return new PaginationVm
        {
            CurrentPage = currentPage,
            TotalPages = totalPages,
            PageSize = pageSize == 0 ? currentItemsCount : pageSize,
            TotalItems = totalItems
        };
    }

    private static string BuildListingUrl(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string listingSlug,
        int listingId)
    {
        return $"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}/{listingSlug}-{listingId}";
    }
}