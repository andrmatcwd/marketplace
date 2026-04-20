using Marketplace.Web.Models.Shared;

namespace Marketplace.Web.Navigation;

public sealed class CatalogBreadcrumbBuilder : ICatalogBreadcrumbBuilder
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogBreadcrumbBuilder(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public IReadOnlyCollection<BreadcrumbItemVm> BuildCatalog(string culture)
    {
        return new List<BreadcrumbItemVm>
        {
            new()
            {
                Title = "Home",
                Url = _urlBuilder.BuildHomeUrl(culture)
            },
            new()
            {
                Title = "Catalog"
            }
        };
    }

    public IReadOnlyCollection<BreadcrumbItemVm> BuildCity(string culture, string cityName, string citySlug)
    {
        return new List<BreadcrumbItemVm>
        {
            new()
            {
                Title = "Home",
                Url = _urlBuilder.BuildHomeUrl(culture)
            },
            new()
            {
                Title = cityName
            }
        };
    }

    public IReadOnlyCollection<BreadcrumbItemVm> BuildCategory(
        string culture,
        string categoryName,
        string categorySlug,
        string cityName,
        string citySlug)
    {
        return new List<BreadcrumbItemVm>
        {
            new()
            {
                Title = "Home",
                Url = _urlBuilder.BuildHomeUrl(culture)
            },
            new()
            {
                Title = cityName,
                Url = _urlBuilder.BuildCityUrl(culture, citySlug)
            },
            new()
            {
                Title = categoryName
            }
        };
    }

    public IReadOnlyCollection<BreadcrumbItemVm> BuildSubCategory(
        string culture,
        string categoryName,
        string categorySlug,
        string subCategoryName,
        string subCategorySlug,
        string cityName,
        string citySlug)
    {
        return new List<BreadcrumbItemVm>
        {
            new()
            {
                Title = "Home",
                Url = _urlBuilder.BuildHomeUrl(culture)
            },
            new()
            {
                Title = cityName,
                Url = _urlBuilder.BuildCityUrl(culture, citySlug)
            },
            new()
            {
                Title = categoryName,
                Url = _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug)
            },
            new()
            {
                Title = subCategoryName
            }
        };
    }

    public IReadOnlyCollection<BreadcrumbItemVm> BuildListing(
        string culture,
        string listingTitle,
        string cityName,
        string citySlug,
        string categoryName,
        string categorySlug,
        string subCategoryName,
        string subCategorySlug)
    {
        return new List<BreadcrumbItemVm>
        {
            new()
            {
                Title = "Home",
                Url = _urlBuilder.BuildHomeUrl(culture)
            },
            new()
            {
                Title = cityName,
                Url = _urlBuilder.BuildCityUrl(culture, citySlug)
            },
            new()
            {
                Title = categoryName,
                Url = _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug)
            },
            new()
            {
                Title = subCategoryName,
                Url = _urlBuilder.BuildSubCategoryUrl(culture, citySlug, categorySlug, subCategorySlug)
            },
            new()
            {
                Title = listingTitle
            }
        };
    }
}