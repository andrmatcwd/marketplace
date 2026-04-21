using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Services.Seo;

public interface ISeoService
{
    PageSeoData BuildHomePageSeo(HomePageVm model, HttpRequest request, string culture);
    PageSeoData BuildCatalogGatewaySeo(CatalogGatewayPageVm model, HttpRequest request, string culture);
    PageSeoData BuildCatalogIndexSeo(CatalogIndexPageVm model, HttpRequest request, string culture);
    PageSeoData BuildCityPageSeo(CityPageVm model, HttpRequest request, string culture);
    PageSeoData BuildCategoryPageSeo(CategoryPageVm model, HttpRequest request, string culture);
    PageSeoData BuildSubCategoryPageSeo(SubCategoryPageVm model, HttpRequest request, string culture);
    PageSeoData BuildListingDetailsSeo(ListingDetailsPageVm model, HttpRequest request, string culture);
}