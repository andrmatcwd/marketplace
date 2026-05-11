using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services.Catalog;

public sealed class CatalogService : ICatalogService
{
    private readonly IMediator _mediator;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;
    private readonly ICatalogUrlBuilder _urlBuilder;
    private readonly ICatalogFilterEnricher _filterEnricher;
    private readonly ICatalogPaginationBuilder _paginationBuilder;

    public CatalogService(
        IMediator mediator,
        ICatalogVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder,
        ICatalogUrlBuilder urlBuilder,
        ICatalogFilterEnricher filterEnricher,
        ICatalogPaginationBuilder paginationBuilder)
    {
        _mediator = mediator;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
        _urlBuilder = urlBuilder;
        _filterEnricher = filterEnricher;
        _paginationBuilder = paginationBuilder;
    }

    public async Task<CatalogGatewayPageVm> GetCatalogGatewayPageAsync(
        string culture,
        string? selectedCitySlug,
        CancellationToken cancellationToken)
    {
        var cities = await _mediator.Send(new GetCatalogCitiesQuery(), cancellationToken);
        var featuredListings = await _mediator.Send(new GetFeaturedListingsQuery(8), cancellationToken);

        var selectedCity = !string.IsNullOrWhiteSpace(selectedCitySlug)
            ? cities.FirstOrDefault(x => string.Equals(x.Slug, selectedCitySlug, StringComparison.OrdinalIgnoreCase))
            : null;

        return new CatalogGatewayPageVm
        {
            Culture = culture,
            SelectedCitySlug = string.IsNullOrWhiteSpace(selectedCitySlug) ? null : selectedCitySlug.Trim(),
            ContinueToCityUrl = selectedCity is null
                ? null
                : _urlBuilder.BuildCityUrl(culture, selectedCity.Slug),
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
                .Select(x => _mapper.MapFilterOption(x.Slug, x.Name))
                .ToList(),
            CitiesSection = new()
            {
                Header = new SectionHeaderVm { Title = "Popular cities", Description = "Choose a city to enter the local service catalog." },
                Items = cities
                    .Take(12)
                    .Select(x => _mapper.MapCityCard(x, culture))
                    .ToList()
            },
            FeaturedListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Popular listings", Description = "Examples of services available in the directory." },
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

        var listingFilter = new CatalogListingFilter
        {
            Search = filter.Search,
            CitySlug = filter.City,
            CategorySlug = filter.Category,
            Sort = filter.Sort,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        var listings = await _mediator.Send(new GetCatalogListingsQuery(listingFilter), cancellationToken);
        var totalCount = await _mediator.Send(new CountCatalogListingsQuery(listingFilter), cancellationToken);

        var cities = await _mediator.Send(new GetCatalogCitiesQuery(Take: 12), cancellationToken);
        var categories = await _mediator.Send(new GetCatalogCategoriesQuery(Take: 12), cancellationToken);

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
                Header = new SectionHeaderVm { Title = "Cities", Description = "Choose a city to browse available services." },
                Items = cities.Select(x => _mapper.MapCityCard(x, culture)).ToList()
            },
            CategoriesSection = new()
            {
                Header = new SectionHeaderVm { Title = "Categories", Description = "Popular service categories in the catalog." },
                Items = categories.Select(x => _mapper.MapCategoryCard(x, culture)).ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Listings and services", Meta = $"Found: {totalCount}" },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = _paginationBuilder.Build(filter, totalCount, page => _urlBuilder.BuildCatalogUrl(culture, page))
            }
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string citySlug,
        CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        var city = await _mediator.Send(new GetCatalogCityBySlugQuery(citySlug), cancellationToken);
        if (city is null) return null;

        filter.City = citySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCityUrl(culture, city.Slug);

        var listingFilter = new CatalogListingFilter
        {
            Search = filter.Search,
            CityId = city.Id,
            CategorySlug = filter.Category,
            Sort = filter.Sort,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        var listings = await _mediator.Send(new GetCatalogListingsQuery(listingFilter), cancellationToken);
        var totalCount = await _mediator.Send(new CountCatalogListingsQuery(listingFilter), cancellationToken);
        var categories = await _mediator.Send(new GetCityCatalogCategoriesQuery(city.Id), cancellationToken);
        var popularSubCategories = await _mediator.Send(new GetPopularCitySubCategoriesQuery(city.Id), cancellationToken);

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
                Header = new SectionHeaderVm { Title = "Categories in the city", Description = $"Popular service categories in {city.Name}" },
                Items = categories.Select(x => _mapper.MapCategoryCard(x, culture, city.Slug)).ToList()
            },
            PopularSubCategoriesSection = new()
            {
                Header = new SectionHeaderVm { Title = "Popular services", Description = $"Go directly to the most popular service directions in {city.Name}." },
                Items = popularSubCategories.Select(x => _mapper.MapSubCategoryCard(x, culture, city.Slug)).ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Listings and services", Meta = $"Found: {totalCount}" },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = _paginationBuilder.Build(filter, totalCount, page => _urlBuilder.BuildCityUrl(culture, city.Slug, page))
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
        var city = await _mediator.Send(new GetCatalogCityBySlugQuery(citySlug), cancellationToken);
        if (city is null) return null;

        var category = await _mediator.Send(new GetCatalogCategoryBySlugQuery(categorySlug), cancellationToken);
        if (category is null) return null;

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug);

        var subCategories = await _mediator.Send(new GetCategorySubCategoriesInCityQuery(city.Id, category.Id), cancellationToken);
        var topListings = await _mediator.Send(new GetTopListingsByCategoryQuery(city.Id, category.Id, filter.Search, 6), cancellationToken);
        var totalCount = await _mediator.Send(new CountListingsByCategoryQuery(city.Id, category.Id, filter.Search), cancellationToken);

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
                Header = new SectionHeaderVm { Title = "Choose a service direction", Description = $"Popular subcategories in {category.Name} for {city.Name}." },
                Items = subCategories.Where(x => x.ListingsCount > 0).Select(x => _mapper.MapSubCategoryCard(x, culture, city.Slug)).ToList()
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Top listings in this category", Description = $"A short selection of popular services in {category.Name}.", Meta = $"Found: {totalCount}" },
                Filter = filter,
                Listings = topListings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = new PaginationVm { CurrentPage = 1, TotalPages = 1 }
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
        var city = await _mediator.Send(new GetCatalogCityBySlugQuery(citySlug), cancellationToken);
        if (city is null) return null;

        var subCategory = await _mediator.Send(new GetCatalogSubCategoryQuery(categorySlug, subCategorySlug), cancellationToken);
        if (subCategory is null) return null;

        filter.City = citySlug;
        filter.Category = categorySlug;
        filter = await _filterEnricher.EnrichAsync(culture, filter, cancellationToken);
        filter.ResetUrl = _urlBuilder.BuildSubCategoryUrl(culture, citySlug, categorySlug, subCategorySlug);

        var listingFilter = new CatalogListingFilter
        {
            Search = filter.Search,
            CityId = city.Id,
            SubCategoryId = subCategory.Id,
            Sort = filter.Sort,
            Page = filter.Page,
            PageSize = filter.PageSize
        };

        var listings = await _mediator.Send(new GetCatalogListingsQuery(listingFilter), cancellationToken);
        var totalCount = await _mediator.Send(new CountCatalogListingsQuery(listingFilter), cancellationToken);

        return new SubCategoryPageVm
        {
            Culture = culture,
            SubCategoryName = subCategory.Name,
            SubCategorySlug = subCategory.Slug,
            CategoryName = subCategory.CategoryName,
            CategorySlug = subCategory.CategorySlug,
            CityName = city.Name,
            CitySlug = city.Slug,
            Hero = new()
            {
                Title = $"{subCategory.Name} in {city.Name}",
                Description = $"Browse offers in {subCategory.Name} in {city.Name}.",
                Breadcrumbs = _breadcrumbBuilder.BuildSubCategory(culture, subCategory.CategoryName, subCategory.CategorySlug, subCategory.Name, subCategory.Slug, city.Name, city.Slug)
            },
            SeoIntro = new()
            {
                Title = $"{subCategory.Name} in {city.Name}",
                Text = $"<p>Available services in direction {subCategory.Name} in {city.Name}.</p>"
            },
            ListingsSection = new()
            {
                Header = new SectionHeaderVm { Title = "Listings and services", Meta = $"Found: {totalCount}" },
                Filter = filter,
                Listings = listings.Select(x => _mapper.MapListingCard(x, culture)).ToList(),
                Pagination = _paginationBuilder.Build(filter, totalCount, page => _urlBuilder.BuildSubCategoryUrl(culture, city.Slug, subCategory.CategorySlug, subCategory.Slug, page))
            }
        };
    }
}
