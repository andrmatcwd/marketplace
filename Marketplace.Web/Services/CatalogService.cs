using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryBySlags;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCityBySlags;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryBySlags;
using Marketplace.Web.Areas.Admin.Models.Listings;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.City;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.SubCategory;
using MediatR;

namespace Marketplace.Web.Services;

public sealed class CatalogService : ICatalogService
{
    private const int DefaultPageSize = 12;
    private const string CatalogTitle = "Каталог";

    private readonly ISender _sender;

    public CatalogService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<CatalogIndexPageVm> GetCatalogIndexPageAsync(
        string culture,
        CancellationToken cancellationToken)
    {
        var popularCitiesTask = _sender.Send(
            new GetCitiesByFilterQuery(CreateCityFilter()),
            cancellationToken);

        var popularCategoriesTask = _sender.Send(
            new GetCategoriesByFilterQuery(CreateCategoryFilter()),
            cancellationToken);

        var popularListingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        await Task.WhenAll(popularCitiesTask, popularCategoriesTask, popularListingsTask);

        var popularCities = await popularCitiesTask;
        var popularCategories = await popularCategoriesTask;
        var popularListings = await popularListingsTask;

        return new CatalogIndexPageVm
        {
            H1 = "Каталог послуг",
            IntroText = "Знайдіть послуги за містом, категорією та підкатегорією.",
            Categories = popularCategories.Items
                .Select(x => MapCategoryItem(x, culture))
                .ToList(),
            Cities = popularCities.Items
                .Select(x => MapCityItem(x, culture))
                .ToList(),
            Listings = popularListings.Items
                .Select(x => MapListingItem(x, culture))
                .ToList(),
            Breadcrumbs = BuildBreadcrumbs(
                culture,
                Crumb(CatalogTitle))
        };
    }

    public async Task<CityPageVm?> GetCityPageAsync(
        string culture,
        string city,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(city), cancellationToken);

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
                .Select(x => MapCategoryItem(x, culture, cityEntity.Slug))
                .ToList(),
            Listings = listings.Items
                .Select(x => MapListingItem(x, culture, citySlug: cityEntity.Slug))
                .ToList(),
            Breadcrumbs = BuildBreadcrumbs(
                culture,
                Crumb(CatalogTitle),
                Crumb(cityEntity.Name, cityEntity.Slug))
        };
    }

    public async Task<CategoryPageVm?> GetCategoryPageAsync(
        string culture,
        string city,
        string category,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(city), cancellationToken);

        // TODO: replace with GetCategoryBySlugQuery(categorySlug)
        var categoryTask = _sender.Send(new GetCategoryBySlagsQuery(city, category), cancellationToken);

        var listingsTask = _sender.Send(
            new GetListingsByFilterQuery(CreateListingFilter()),
            cancellationToken);

        // TODO: replace with GetSubCategoriesByFilterQuery(...)
        var subCategoriesTask = _sender.Send(
            new GetCategoriesByFilterQuery(CreateCategoryFilter()),
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
                .Select(x => new SubCategoryItemVm
                {
                    Name = x.Name,
                    Slug = x.Slug,
                    ListingsCount = x.ListingsCount,
                    Url = BuildUrl(
                        culture: culture,
                        citySlug: cityEntity.Slug,
                        categorySlug: categoryEntity.Slug,
                        subCategorySlug: x.Slug)
                })
                .ToList(),
            Listings = listings.Items
                .Select(x => MapListingItem(
                    x,
                    culture,
                    citySlug: cityEntity.Slug,
                    categorySlug: categoryEntity.Slug))
                .ToList(),
            Breadcrumbs = BuildBreadcrumbs(
                culture,
                Crumb(CatalogTitle),
                Crumb(cityEntity.Name, cityEntity.Slug),
                Crumb(categoryEntity.Name, categoryEntity.Slug))
        };
    }

    public async Task<SubCategoryPageVm?> GetSubCategoryPageAsync(
        string culture,
        string city,
        string category,
        string subCategory,
        BaseFilter filter,
        CancellationToken cancellationToken)
    {
        // TODO: replace with GetCityBySlugQuery(city)
        var cityTask = _sender.Send(new GetCityBySlagsQuery(city), cancellationToken);

        // TODO: replace with GetCategoryBySlugQuery(categorySlug)
        var categoryTask = _sender.Send(new GetCategoryBySlagsQuery(city, category), cancellationToken);

        // TODO: replace with GetSubCategoryBySlugQuery(subCategorySlug)
        var subCategoryTask = _sender.Send(new GetSubCategoryBySlagsQuery(city, category, subCategory), cancellationToken);

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
            SubcategoryName = subCategoryEntity.Name,
            SubcategorySlug = subCategoryEntity.Slug,
            H1 = $"{subCategoryEntity.Name} у {cityEntity.Name}",
            IntroText = subCategoryEntity.Description,
            TotalListingsCount = subCategoryEntity.ListingsCount,
            Listings = listings.Items
                .Select(x => MapListingItem(
                    x,
                    culture,
                    citySlug: cityEntity.Slug,
                    categorySlug: categoryEntity.Slug,
                    subCategorySlug: subCategoryEntity.Slug))
                .ToList(),
            Breadcrumbs = BuildBreadcrumbs(
                culture,
                Crumb(CatalogTitle),
                Crumb(cityEntity.Name, cityEntity.Slug),
                Crumb(categoryEntity.Name, categoryEntity.Slug),
                Crumb(subCategoryEntity.Name, subCategoryEntity.Slug))
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

    private static ListingFilter CreateListingFilter()
    {
        return new ListingFilter
        {
            Page = 1,
            PageSize = DefaultPageSize
        };
    }

    private static CategoryItemVm MapCategoryItem(
        dynamic x,
        string culture,
        string? citySlug = null)
    {
        return new CategoryItemVm
        {
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Icon = x.Icon,
            ListingsCount = x.ListingsCount,
            SubCategoryCount = x.SubCategoriesCount,
            Url = BuildUrl(
                culture: culture,
                citySlug: citySlug,
                categorySlug: x.Slug)
        };
    }

    private static CityItemVm MapCityItem(dynamic x, string culture)
    {
        return new CityItemVm
        {
            Name = x.Name,
            Slug = x.Slug,
            RegionName = x.RegionName,
            RegionSlug = x.RegionSlug,
            ListingsCount = x.ListingsCount,
            CategoriesCount = x.CategoriesCount,
            Url = BuildUrl(
                culture: culture,
                citySlug: x.Slug)
        };
    }

    private static ListingListItemVm MapListingItem(
        dynamic x,
        string culture,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null)
    {
        return new ListingListItemVm
        {
            Id = x.Id,
            Title = x.Title,
            Slug = x.Slug,
            Description = x.Description,
            Url = BuildUrl(
                culture: culture,
                citySlug: citySlug,
                categorySlug: categorySlug,
                subCategorySlug: subCategorySlug,
                listingSlug: x.Slug,
                listingId: x.Id)
        };
    }

    private static string BuildUrl(
        string? culture = null,
        string? citySlug = null,
        string? categorySlug = null,
        string? subCategorySlug = null,
        string? listingSlug = null,
        int? listingId = null)
    {
        var parts = new List<string>();

        AddIfHasValue(parts, culture);
        AddIfHasValue(parts, citySlug);
        AddIfHasValue(parts, categorySlug);
        AddIfHasValue(parts, subCategorySlug);

        if (!string.IsNullOrWhiteSpace(listingSlug) && listingId.HasValue)
        {
            parts.Add($"{listingSlug}-{listingId.Value}");
        }
        else if (!string.IsNullOrWhiteSpace(listingSlug))
        {
            parts.Add(listingSlug);
        }
        else if (listingId.HasValue)
        {
            parts.Add(listingId.Value.ToString());
        }

        return "/" + string.Join("/", parts);
    }

    private static IReadOnlyList<BreadcrumbItemVm> BuildBreadcrumbs(
        string? culture,
        params BreadcrumbSegment[] segments)
    {
        var validSegments = segments
            .Where(x => !string.IsNullOrWhiteSpace(x.Title))
            .ToList();

        if (validSegments.Count == 0)
            return [];

        var items = new List<BreadcrumbItemVm>(validSegments.Count);

        for (var i = 0; i < validSegments.Count; i++)
        {
            items.Add(new BreadcrumbItemVm
            {
                Title = validSegments[i].Title,
                Url = i == validSegments.Count - 1
                    ? null
                    : BuildBreadcrumbUrl(culture, validSegments, i),
                IsCurrent = i == validSegments.Count - 1
            });
        }

        return items;
    }

    private static string BuildBreadcrumbUrl(
        string? culture,
        IReadOnlyList<BreadcrumbSegment> segments,
        int lastIndexInclusive)
    {
        var parts = new List<string>();

        AddIfHasValue(parts, culture);

        for (var i = 0; i <= lastIndexInclusive; i++)
        {
            AddIfHasValue(parts, segments[i].Slug);
        }

        return "/" + string.Join("/", parts);
    }

    private static BreadcrumbSegment Crumb(string title, string? slug = null)
    {
        return new BreadcrumbSegment(title, slug);
    }

    private static void AddIfHasValue(List<string> parts, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            parts.Add(value);
        }
    }

    private sealed record BreadcrumbSegment(string Title, string? Slug);
}