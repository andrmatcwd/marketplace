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
        => _urlBuilder.BuildHomeUrl(culture);

    public string BuildCatalog(string culture, int page = 1)
        => _urlBuilder.BuildCatalogUrl(culture, page);

    public string BuildCity(CityPageVm model)
        => _urlBuilder.BuildCityUrl(model.Culture, model.CitySlug, model.ListingsSection.Filter.Page);

    public string BuildCategory(CategoryPageVm model)
        => _urlBuilder.BuildCategoryUrl(
            model.Culture,
            model.CitySlug,
            model.CategorySlug,
            model.ListingsSection.Filter.Page);

    public string BuildSubCategory(SubCategoryPageVm model)
        => _urlBuilder.BuildSubCategoryUrl(
            model.Culture,
            model.CitySlug,
            model.CategorySlug,
            model.SubCategorySlug,
            model.ListingsSection.Filter.Page);

    public string BuildListing(ListingDetailsPageVm model)
        => _urlBuilder.BuildListingUrl(
            model.Culture,
            model.CitySlug!,
            model.CategorySlug!,
            model.SubCategorySlug!,
            model.Slug,
            model.Id);
}