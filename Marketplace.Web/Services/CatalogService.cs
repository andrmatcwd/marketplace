using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryBySlags;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCityBySlags;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryBySlags;
using Marketplace.Web.Areas.Admin.Models.Listings;
using Marketplace.Web.Mappings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Models.SubCategory;
using Marketplace.Web.Navigation;
using MediatR;

namespace Marketplace.Web.Services;

public sealed class CatalogService : ICatalogService
{
    private const int DefaultPageSize = 12;
    private const string CatalogTitle = "Каталог";

    private readonly ISender _sender;
    private readonly ICatalogVmMapper _mapper;
    private readonly ICatalogBreadcrumbBuilder _breadcrumbBuilder;

    public CatalogService(
        ISender sender,
        ICatalogVmMapper mapper,
        ICatalogBreadcrumbBuilder breadcrumbBuilder)
    {
        _sender = sender;
        _mapper = mapper;
        _breadcrumbBuilder = breadcrumbBuilder;
    }


    public async Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(
        string culture,
        CancellationToken cancellationToken)
    {
        var citiesTask = _sender.Send(
            new GetCitiesByFilterQuery(CreateCityFilter()),
            cancellationToken);

        var categoriesTask = _sender.Send(
            new GetCategoriesByFilterQuery(CreateCategoryFilter()),
            cancellationToken);

        var listingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        await Task.WhenAll(citiesTask, categoriesTask, listingsTask);

        var cities = await citiesTask;
        var categories = await categoriesTask;
        var listings = await listingsTask;

        return new CatalogIndexPageVm
        {
            H1 = "Каталог послуг",
            IntroText = "Знайдіть послуги за містом, категорією та підкатегорією.",
            Cities = cities.Items
                .Select(x => _mapper.MapCity(x, culture))
                .ToList(),
            Categories = categories.Items
                .Select(x => _mapper.MapCategory(x, culture))
                .ToList(),
            Listings = listings.Items
                .Select(x => _mapper.MapListing(x, culture))
                .ToList(),
            Breadcrumbs = _breadcrumbBuilder.Build(
                culture,
                BreadcrumbSegments.Root(CatalogTitle))
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string citySlag,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(citySlag), cancellationToken);

        var categoriesTask = _sender.Send(
            new GetCategoriesByFilterQuery(CreateCategoryFilter()),
            cancellationToken);

        var listingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        await Task.WhenAll(cityTask, categoriesTask, listingsTask);

        var cityEntity = await cityTask;
        var categories = await categoriesTask;
        var listings = await listingsTask;

        return new CityPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            H1 = $"Послуги у {cityEntity.Name}",
            IntroText = $"Перегляньте доступні категорії та популярні пропозиції у місті {cityEntity.Name}.",
            TotalListingsCount = cityEntity.ListingsCount,
            TotalCategoriesCount = cityEntity.CategoriesCount,
            Categories = categories.Items
                .Select(x => _mapper.MapCategory(x, culture, cityEntity.Slug))
                .ToList(),
            Listings = listings.Items
                .Select(x => _mapper.MapListing(x, culture, citySlug: cityEntity.Slug))
                .ToList(),
            Breadcrumbs = _breadcrumbBuilder.Build(
                culture,
                BreadcrumbSegments.Root(CatalogTitle),
                BreadcrumbSegments.City(cityEntity.Name, cityEntity.Slug))
        };
    }

    public async Task<CategoryPageVm?> GetCategoryPageAsync(
        string culture,
        string citySlag,
        string categorySlag,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(citySlag), cancellationToken);

        // TODO: replace with GetCategoryBySlugQuery(categorySlug)
        var categoryTask = _sender.Send(new GetCategoryBySlagsQuery(citySlag, categorySlag), cancellationToken);

        var listingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        // TODO: replace with GetSubCategoriesByFilterQuery(...)
        var subCategoriesTask = _sender.Send(
            new GetSubCategoriesByFilterQuery(CreateSubCategoryFilter()),
            cancellationToken);

        await Task.WhenAll(cityTask, categoryTask, listingsTask, subCategoriesTask);

        var cityEntity = await cityTask;
        var categoryEntity = await categoryTask;
        var listings = await listingsTask;
        var subCategories = await subCategoriesTask;

        return new CategoryPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            CategoryName = categoryEntity.Name,
            CategorySlug = categoryEntity.Slug,
            H1 = $"{categoryEntity.Name} у {cityEntity.Name}",
            IntroText = categoryEntity.Description,
            TotalListingsCount = categoryEntity.ListingsCount,
            SubCategories = subCategories.Items
                .Select(x => _mapper.MapSubCategory(
                    x,
                    culture,
                    citySlug: cityEntity.Slug,
                    categorySlug: categoryEntity.Slug))
                .ToList(),
            Listings = listings.Items
                .Select(x => _mapper.MapListing(
                    x,
                    culture,
                    citySlug: cityEntity.Slug,
                    categorySlug: categoryEntity.Slug))
                .ToList(),
            Breadcrumbs = _breadcrumbBuilder.Build(
                culture,
                BreadcrumbSegments.Root(CatalogTitle),
                BreadcrumbSegments.City(cityEntity.Name, cityEntity.Slug),
                BreadcrumbSegments.Category(categoryEntity.Name, categoryEntity.Slug))
        };
    }

    public async Task<SubCategoryPageVm?> GetSubCategoryPageAsync(
        string culture,
        string citySlag,
        string categorySlag,
        string subCategorySlag,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(citySlag), cancellationToken);

        // TODO: replace with GetCategoryBySlugQuery(categorySlug)
        var categoryTask = _sender.Send(new GetCategoryBySlagsQuery(citySlag, categorySlag), cancellationToken);

        // TODO: replace with GetSubCategoryBySlugQuery(subCategorySlug)
        var subCategoryTask = _sender.Send(new GetSubCategoryBySlagsQuery(citySlag, categorySlag, subCategorySlag), cancellationToken);

        var listingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        await Task.WhenAll(cityTask, categoryTask, subCategoryTask, listingsTask);

        var cityEntity = await cityTask;
        var categoryEntity = await categoryTask;
        var subCategoryEntity = await subCategoryTask;
        var listings = await listingsTask;

        return new SubCategoryPageVm
        {
            CityName = cityEntity.Name,
            CitySlug = cityEntity.Slug,
            CategoryName = categoryEntity.Name,
            CategorySlug = categoryEntity.Slug,
            SubCategoryName = subCategoryEntity.Name,
            SubCategorySlug = subCategoryEntity.Slug,
            H1 = $"{subCategoryEntity.Name} у {cityEntity.Name}",
            IntroText = subCategoryEntity.Description,
            TotalListingsCount = subCategoryEntity.ListingsCount,
            Listings = listings.Items
                .Select(x => _mapper.MapListing(
                    x,
                    culture,
                    citySlug: cityEntity.Slug,
                    categorySlug: categoryEntity.Slug,
                    subCategorySlug: subCategoryEntity.Slug))
                .ToList(),
            Breadcrumbs = _breadcrumbBuilder.Build(
                culture,
                BreadcrumbSegments.Root(CatalogTitle),
                BreadcrumbSegments.City(cityEntity.Name, cityEntity.Slug),
                BreadcrumbSegments.Category(categoryEntity.Name, categoryEntity.Slug),
                BreadcrumbSegments.SubCategory(subCategoryEntity.Name, subCategoryEntity.Slug))
        };
    }

    private static CityFilter CreateCityFilter()
    {
        return new CityFilter
        {
            Page = 1,
            PageSize = DefaultPageSize
        };
    }

    private static CategoryFilter CreateCategoryFilter()
    {
        return new CategoryFilter
        {
            Page = 1,
            PageSize = DefaultPageSize
        };
    }

    private static SubCategoryFilter CreateSubCategoryFilter()
    {
        return new SubCategoryFilter
        {
            Page = 1,
            PageSize = DefaultPageSize
        };
    }

    private static ListingFilter CreateListingFilter()
    {
        return new ListingFilter
        {
            Page = 1,
            PageSize = DefaultPageSize
        };
    }
}