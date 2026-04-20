using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Navigation;

namespace Marketplace.Web.Seo;

public sealed class CanonicalUrlBuilder
{
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CanonicalUrlBuilder(ICatalogUrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;
    }

    public string BuildHome(string culture)
    {
        return _urlBuilder.BuildHomeUrl(culture);
    }

    public string BuildCatalog(string culture)
    {
        return _urlBuilder.BuildCatalogUrl(culture);
    }

    public string BuildCity(CityPageVm model)
    {
        return _urlBuilder.BuildCityUrl(model.Culture, model.CitySlug);
    }

    public string BuildCategory(CategoryPageVm model)
    {
        return _urlBuilder.BuildCategoryUrl(model.Culture, model.CitySlug!, model.CategorySlug);
    }

    public string BuildSubCategory(SubCategoryPageVm model)
    {
        return _urlBuilder.BuildSubCategoryUrl(model.Culture, model.CitySlug!, model.CategorySlug!, model.SubCategorySlug);
    }

    public string BuildListing(ListingDetailsPageVm model)
    {
        if (string.IsNullOrWhiteSpace(model.CitySlug) ||
            string.IsNullOrWhiteSpace(model.CategorySlug) ||
            string.IsNullOrWhiteSpace(model.SubCategorySlug))
        {
            return model.Url;
        }

        return _urlBuilder.BuildListingUrl(
            model.Culture,
            model.CitySlug,
            model.CategorySlug,
            model.SubCategorySlug,
            model.Slug,
            model.Id);
    }
}