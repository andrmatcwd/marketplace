using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Microsoft.AspNetCore.Http;

namespace Marketplace.Web.Services.Seo;

public sealed class SeoService : ISeoService
{
    private readonly CanonicalUrlBuilder _canonicalUrlBuilder;
    private readonly IAbsoluteUrlBuilder _absoluteUrlBuilder;
    private readonly SeoPaginationBuilder _seoPaginationBuilder;
    private readonly SeoIndexingPolicy _seoIndexingPolicy;

    public SeoService(
        CanonicalUrlBuilder canonicalUrlBuilder,
        IAbsoluteUrlBuilder absoluteUrlBuilder,
        SeoPaginationBuilder seoPaginationBuilder,
        SeoIndexingPolicy seoIndexingPolicy)
    {
        _canonicalUrlBuilder = canonicalUrlBuilder;
        _absoluteUrlBuilder = absoluteUrlBuilder;
        _seoPaginationBuilder = seoPaginationBuilder;
        _seoIndexingPolicy = seoIndexingPolicy;
    }

    public PageSeoData BuildHomePageSeo(HomePageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildHome(culture));

        return new PageSeoData
        {
            Title = "Service catalog — Marketplace",
            Description = "A clean service catalog by city, category, and subcategory.",
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = "Service catalog — Marketplace",
            OgDescription = "A clean service catalog by city, category, and subcategory.",
            OgType = "website",
            OgUrl = canonical,
            Robots = "index, follow"
        };
    }

    public PageSeoData BuildCatalogIndexSeo(CatalogIndexPageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildCatalog(culture));
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        return new PageSeoData
        {
            Title = "Catalog of services",
            Description = "Browse services by city and category.",
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = "Catalog of services",
            OgDescription = "Browse services by city and category.",
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination)
        };
    }

    public PageSeoData BuildCityPageSeo(CityPageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildCity(model));
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        return new PageSeoData
        {
            Title = $"Services in {model.CityName}",
            Description = $"Find available services in {model.CityName}.",
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = $"Services in {model.CityName}",
            OgDescription = $"Find available services in {model.CityName}.",
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination)
        };
    }

    public PageSeoData BuildCategoryPageSeo(CategoryPageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildCategory(model));
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = !string.IsNullOrWhiteSpace(model.CityName)
            ? $"{model.CategoryName} in {model.CityName}"
            : $"{model.CategoryName} — services";

        return new PageSeoData
        {
            Title = title,
            Description = $"Available services in category {model.CategoryName}.",
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = $"Available services in category {model.CategoryName}.",
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination)
        };
    }

    public PageSeoData BuildSubCategoryPageSeo(SubCategoryPageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildSubCategory(model));
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = !string.IsNullOrWhiteSpace(model.CityName)
            ? $"{model.SubCategoryName} in {model.CityName}"
            : $"{model.SubCategoryName} — services";

        return new PageSeoData
        {
            Title = title,
            Description = $"Browse offers in direction {model.SubCategoryName}.",
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = $"Browse offers in direction {model.SubCategoryName}.",
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination)
        };
    }

    public PageSeoData BuildListingDetailsSeo(ListingDetailsPageVm model, HttpRequest request, string culture)
    {
        var canonical = _absoluteUrlBuilder.Build(request, _canonicalUrlBuilder.BuildListing(model));

        var image = model.Gallery.Images.FirstOrDefault(x => x.IsPrimary)?.Url
                    ?? model.Gallery.Images.FirstOrDefault()?.Url;

        return new PageSeoData
        {
            Title = model.Title,
            Description = model.ShortDescription,
            CanonicalUrl = canonical,
            H1 = model.Title,
            SeoText = model.Description,
            OgTitle = model.Title,
            OgDescription = model.ShortDescription,
            OgImage = image is null ? null : _absoluteUrlBuilder.Build(request, image),
            OgType = "article",
            OgUrl = canonical,
            Robots = "index, follow"
        };
    }
}