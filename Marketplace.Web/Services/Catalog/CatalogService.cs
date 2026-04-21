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
    private readonly ICatalogFilterEnricher _filterEnricher;
    private readonly ICatalogLookupService _lookupService;
    private readonly ICatalogPaginationBuilder _paginationBuilder;

    public CatalogService(
        ApplicationDbContext dbContext,
        ICatalogVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder,
        ICatalogUrlBuilder urlBuilder,
        ICatalogFilterEnricher filterEnricher,
        ICatalogLookupService lookupService,
        ICatalogPaginationBuilder paginationBuilder)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
        _urlBuilder = urlBuilder;
        _filterEnricher = filterEnricher;
        _lookupService = lookupService;
        _paginationBuilder = paginationBuilder;
    }

    public async Task<CatalogGatewayPageVm> GetCatalogGatewayPageAsync(
        string culture,
        string? selectedCitySlug,
        CancellationToken cancellationToken)
    {
        var cities = await _lookupService.GetPublishedCitiesAsync(cancellationToken);

        var featuredListings = await _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .ApplySorting("rating")
            .Take(8)
            .ToListAsync(cancellationToken);

        var selectedCity = !string.IsNullOrWhiteSpace(selectedCitySlug)
            ? cities.FirstOrDefault(x => string.Equals(x.Entity.Slug, selectedCitySlug, StringComparison.OrdinalIgnoreCase))
            : null;

        return new CatalogGatewayPageVm
        {
            Culture = culture,
            SelectedCitySlug = string.IsNullOrWhiteSpace(selectedCitySlug)
                ? null
                : selectedCitySlug.Trim(),
            ContinueToCityUrl = selectedCity is null
                ? null
                : _urlBuilder.BuildCityUrl(culture, selectedCity.Entity.Slug),
            Hero = new()
            {
                Title = "Choose a city to find services",
                Description = "Select a city first to browse relevant categories, subcategories, and providers.",
                Breadcrumbs = _breadcrumbBuilder.BuildCatalog(culture)
            },
            SeoIntro = new()
            {
                Title = "How the catalog works",
                Text = "<p>Choose a city first to see relevant services, categories, and specialists. After selecting a city, you can browse local categories and open the most relevant listings.</p>"
            },
            CityOptions = cities
                .Select(x => _mapper.MapFilterOption(x.Entity.Slug, x.Entity.Name))
                .ToList(),
            CitiesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Popular cities",
                    Description = "Choose a city to enter the local service catalog."
                },
                Items = cities
                    .Take(12)
                    .Select(x => _mapper.MapCityCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            FeaturedListingsSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Popular listings",
                    Description = "Examples of services available in the directory."
                },
                ShowMobileFilterButton = false,
                Listings = featuredListings
                    .Select(x => _mapper.MapListingCard(x, culture))
                    .ToList()
            }
        };
    }

    public async Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(
        string culture,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
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

        var cities = await _lookupService.GetPublishedCitiesAsync(cancellationToken, take: 12);
        var categories = await _lookupService.GetPublishedCategoriesAsync(cancellationToken, take: 12);

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
                Items = cities
                    .Select(x => _mapper.MapCityCard(x.Entity, x.ListingsCount, culture))
                    .ToList()
            },
            CategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Categories",
                    Description = "Popular service categories in the catalog."
                },
                Items = categories
                    .Select(x => _mapper.MapCategoryCard(x.Entity, x.ListingsCount, culture))
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
                Listings = listings
                    .Select(x => _mapper.MapListingCard(x, culture))
                    .ToList(),
                Pagination = _paginationBuilder.Build(
                    filter,
                    totalCount,
                    page => _urlBuilder.BuildCatalogUrl(culture, page))
            }
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string citySlug,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        var city = await _lookupService.GetPublishedCityBySlugAsync(citySlug, cancellationToken);
        if (city is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
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

        var categories = await _lookupService.GetCityCategoriesAsync(city.Id, cancellationToken);
        var popularSubCategories = await _lookupService.GetPopularCitySubCategoriesAsync(city.Id, cancellationToken);

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
                Items = categories
                    .Select(x => _mapper.MapCategoryCard(x.Entity, x.ListingsCount, culture, city.Slug))
                    .ToList()
            },
            PopularSubCategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Popular services",
                    Description = $"Go directly to the most popular service directions in {city.Name}."
                },
                Items = popularSubCategories
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
                Listings = listings
                    .Select(x => _mapper.MapListingCard(x, culture))
                    .ToList(),
                Pagination = _paginationBuilder.Build(
                    filter,
                    totalCount,
                    page => _urlBuilder.BuildCityUrl(culture, city.Slug, page))
            }
        };
    }

    public async Task<CategoryPageVm?> GetCategoryPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        var city = await _lookupService.GetPublishedCityBySlugAsync(citySlug, cancellationToken);
        if (city is null)
        {
            return null;
        }

        var category = await _lookupService.GetPublishedCategoryBySlugAsync(categorySlug, cancellationToken);
        if (category is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug);

        var subCategories = await _lookupService.GetCategorySubCategoriesInCityAsync(
            city.Id,
            category.Id,
            cancellationToken);

        var topListings = await _dbContext.Listings
            .AsNoTracking()
            .Published()
            .WithCatalogIncludes()
            .Where(x => x.CityId == city.Id && x.CategoryId == category.Id)
            .ApplySearch(filter.Search)
            .ApplySorting("rating")
            .Take(6)
            .ToListAsync(cancellationToken);

        var totalCount = await _dbContext.Listings
            .AsNoTracking()
            .Published()
            .Where(x => x.CityId == city.Id && x.CategoryId == category.Id)
            .ApplySearch(filter.Search)
            .CountAsync(cancellationToken);

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
                Description = $"Choose a specific service direction in {category.Name} for {city.Name}.",
                Breadcrumbs = _breadcrumbBuilder.BuildCategory(culture, category.Name, category.Slug, city.Name, city.Slug)
            },
            SeoIntro = new()
            {
                Title = $"{category.Name} in {city.Name}",
                Text = $"<p>Browse the main service directions in category {category.Name} for {city.Name}. Choose a subcategory to see more relevant listings and offers.</p>"
            },
            SubCategoriesSection = new()
            {
                Header = new SectionHeaderVm
                {
                    Title = "Choose a service direction",
                    Description = $"Popular subcategories in {category.Name} for {city.Name}."
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
                    Title = "Top listings in this category",
                    Description = $"A short selection of popular services in {category.Name}.",
                    Meta = $"Found: {totalCount}"
                },
                Filter = filter,
                Listings = topListings
                    .Select(x => _mapper.MapListingCard(x, culture))
                    .ToList(),
                Pagination = new PaginationVm
                {
                    CurrentPage = 1,
                    TotalPages = 1
                }
            }
        };
    }

    public async Task<SubCategoryPageVm?> GetSubCategoryPageAsync(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        var city = await _lookupService.GetPublishedCityBySlugAsync(citySlug, cancellationToken);
        if (city is null)
        {
            return null;
        }

        var subCategory = await _lookupService.GetPublishedSubCategoryWithCategoryAsync(
            categorySlug,
            subCategorySlug,
            cancellationToken);

        if (subCategory is null || subCategory.Category is null)
        {
            return null;
        }

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
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
                Listings = listings
                    .Select(x => _mapper.MapListingCard(x, culture))
                    .ToList(),
                Pagination = _paginationBuilder.Build(
                    filter,
                    totalCount,
                    page => _urlBuilder.BuildSubCategoryUrl(
                        culture,
                        city.Slug,
                        subCategory.Category.Slug,
                        subCategory.Slug,
                        page))
            }
        };
    }
}