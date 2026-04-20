using Marketplace.Web.Data;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogService : ICatalogService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogService(
        ApplicationDbContext dbContext,
        ICatalogVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder,
        ICatalogUrlBuilder urlBuilder)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
        _urlBuilder = urlBuilder;
    }

    public async Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(string culture, CatalogFilterVm filter, CancellationToken cancellationToken)
    {
        filter = await EnrichFilterAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCatalogUrl(culture);

        var query = _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .ApplySearch(filter.Search)
            .ApplyCity(filter.City)
            .ApplyCategory(filter.Category);

        var totalCount = await query.CountAsync(cancellationToken);

        var listings = await query
            .ApplySorting(filter.Sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CityId == x.Id && l.IsPublished)
            })
            .Take(12)
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = _dbContext.Listings.Count(l => l.CategoryId == x.Id && l.IsPublished)
            })
            .Take(12)
            .ToListAsync(cancellationToken);

        return new CatalogIndexPageVm
        {
            Culture = culture,
            Hero = new()
            {
                Title = "Catalog of services",
                Description = "Find trusted services in your city.",
                Breadcrumbs = _breadcrumbBuilder.BuildCatalog(culture)
            },
            SeoIntro = new()
            {
                Title = "Catalog",
                Text = "<p>Select a city, category, or use the filter to find a specific service.</p>"
            },
            CitiesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Cities",
                    Description = "Choose a city to browse available services."
                },
                Items = cities.Select(x => _mapper.MapCityCard(x.Entity, x.ListingsCount, culture)).ToList()
            },
            CategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Categories",
                    Description = "Popular service categories in the catalog."
                },
                Items = categories.Select(x => _mapper.MapCategoryCard(x.Entity, x.ListingsCount, culture)).ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Listings and services",
                    Meta = $"Found: {totalCount}"
                },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = BuildPagination(filter, totalCount, _urlBuilder.BuildCatalogUrl(culture))
            }
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(string culture, string citySlug, CatalogFilterVm filter, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == citySlug, cancellationToken);

        if (city is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter = await EnrichFilterAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCityUrl(culture, city.Slug);

        var query = _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .Where(x => x.CityId == city.Id)
            .ApplySearch(filter.Search)
            .ApplyCategory(filter.Category);

        var totalCount = await query.CountAsync(cancellationToken);

        var listings = await query
            .ApplySorting(filter.Sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Listings.Any(l => l.CityId == city.Id && l.IsPublished))
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = x.Listings.Count(l => l.CityId == city.Id && l.IsPublished)
            })
            .ToListAsync(cancellationToken);

        return new CityPageVm
        {
            Culture = culture,
            CityName = city.Name,
            CitySlug = city.Slug,
            Hero = new()
            {
                Title = $"Services in {city.Name}",
                Description = $"Browse available services and specialists in {city.Name}.",
                Breadcrumbs = _breadcrumbBuilder.BuildCity(culture, city.Name, city.Slug)
            },
            SeoIntro = new()
            {
                Title = $"Service catalog in {city.Name}",
                Text = $"<p>Find актуальні services, companies and specialists in {city.Name}.</p>"
            },
            CategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Categories in the city",
                    Description = $"Popular service categories in {city.Name}"
                },
                Items = categories.Select(x => _mapper.MapCategoryCard(x.Entity, x.ListingsCount, culture, city.Slug)).ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Listings and services",
                    Meta = $"Found: {totalCount}"
                },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = BuildPagination(filter, totalCount, _urlBuilder.BuildCityUrl(culture, city.Slug))
            }
        };
    }

    public async Task<CategoryPageVm?> GetCategoryPageAsync(string culture, string citySlug, string categorySlug, CatalogFilterVm filter, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == citySlug, cancellationToken);

        if (city is null)
        {
            return null;
        }

        var category = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == categorySlug, cancellationToken);

        if (category is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await EnrichFilterAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug);

        var query = _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .Where(x => x.CityId == city.Id && x.CategoryId == category.Id)
            .ApplySearch(filter.Search);

        var totalCount = await query.CountAsync(cancellationToken);

        var listings = await query
            .ApplySorting(filter.Sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var subCategories = await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.IsPublished && x.CategoryId == category.Id)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new
            {
                Entity = x,
                ListingsCount = x.Listings.Count(l => l.IsPublished && l.CityId == city.Id)
            })
            .ToListAsync(cancellationToken);

        return new CategoryPageVm
        {
            Culture = culture,
            CategoryName = category.Name,
            CategorySlug = category.Slug,
            CityName = city.Name,
            CitySlug = city.Slug,
            Hero = new()
            {
                Title = $"{category.Name} in {city.Name}",
                Description = $"Browse services in category {category.Name} in {city.Name}.",
                Breadcrumbs = _breadcrumbBuilder.BuildCategory(culture, category.Name, category.Slug, city.Name, city.Slug)
            },
            SeoIntro = new()
            {
                Title = $"{category.Name} in {city.Name}",
                Text = $"<p>Browse available services in category {category.Name} for {city.Name}.</p>"
            },
            SubCategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Subcategories",
                    Description = "Choose a more specific service direction."
                },
                Items = subCategories
                    .Where(x => x.ListingsCount > 0)
                    .Select(x => _mapper.MapSubCategoryCard(x.Entity, x.ListingsCount, culture, city.Slug))
                    .ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Listings and services",
                    Meta = $"Found: {totalCount}"
                },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = BuildPagination(filter, totalCount, _urlBuilder.BuildCategoryUrl(culture, city.Slug, category.Slug))
            }
        };
    }

    public async Task<SubCategoryPageVm?> GetSubCategoryPageAsync(string culture, string citySlug, string categorySlug, string subCategorySlug, CatalogFilterVm filter, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IsPublished && x.Slug == citySlug, cancellationToken);

        if (city is null)
        {
            return null;
        }

        var subCategory = await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.IsPublished &&
                     x.Slug == subCategorySlug &&
                     x.Category != null &&
                     x.Category.Slug == categorySlug,
                cancellationToken);

        if (subCategory is null || subCategory.Category is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await EnrichFilterAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildSubCategoryUrl(culture, citySlug, categorySlug, subCategorySlug);

        var query = _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .Where(x => x.CityId == city.Id && x.SubCategoryId == subCategory.Id)
            .ApplySearch(filter.Search);

        var totalCount = await query.CountAsync(cancellationToken);

        var listings = await query
            .ApplySorting(filter.Sort)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        return new SubCategoryPageVm
        {
            Culture = culture,
            SubCategoryName = subCategory.Name,
            SubCategorySlug = subCategory.Slug,
            CategoryName = subCategory.Category.Name,
            CategorySlug = subCategory.Category.Slug,
            CityName = city.Name,
            CitySlug = city.Slug,
            Hero = new()
            {
                Title = $"{subCategory.Name} in {city.Name}",
                Description = $"Browse offers in {subCategory.Name} in {city.Name}.",
                Breadcrumbs = _breadcrumbBuilder.BuildSubCategory(
                    culture,
                    subCategory.Category.Name,
                    subCategory.Category.Slug,
                    subCategory.Name,
                    subCategory.Slug,
                    city.Name,
                    city.Slug)
            },
            SeoIntro = new()
            {
                Title = $"{subCategory.Name} in {city.Name}",
                Text = $"<p>Available services in direction {subCategory.Name} in {city.Name}.</p>"
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Listings and services",
                    Meta = $"Found: {totalCount}"
                },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = BuildPagination(
                    filter,
                    totalCount,
                    _urlBuilder.BuildSubCategoryUrl(culture, city.Slug, subCategory.Category.Slug, subCategory.Slug))
            }
        };
    }

    private async Task<CatalogFilterVm> EnrichFilterAsync(string culture, CatalogFilterVm filter, CancellationToken cancellationToken)
    {
        filter.Page = filter.Page < 1 ? 1 : filter.Page;
        filter.PageSize = filter.PageSize <= 0 ? 12 : Math.Min(filter.PageSize, 60);

        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new { x.Slug, x.Name })
            .ToListAsync(cancellationToken);

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Name)
            .Select(x => new { x.Slug, x.Name })
            .ToListAsync(cancellationToken);

        filter.Cities = cities
            .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
            .ToList();

        filter.Categories = categories
            .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
            .ToList();

        filter.SortOptions = new List<FilterOptionVm>
        {
            _mapper.MapFilterOption("newest", "Newest first"),
            _mapper.MapFilterOption("rating", "By rating"),
            _mapper.MapFilterOption("title", "By title")
        };

        filter.ResetUrl = _urlBuilder.BuildCatalogUrl(culture);

        return filter;
    }

    private static PaginationVm BuildPagination(CatalogFilterVm filter, int totalItems, string baseUrl)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)filter.PageSize);

        return new PaginationVm
        {
            CurrentPage = filter.Page,
            TotalPages = totalPages,
            BaseUrl = baseUrl,
            Query = new Dictionary<string, string?>
            {
                ["search"] = filter.Search,
                ["city"] = filter.City,
                ["category"] = filter.Category,
                ["sort"] = filter.Sort,
                ["pageSize"] = filter.PageSize.ToString()
            }
        };
    }
}