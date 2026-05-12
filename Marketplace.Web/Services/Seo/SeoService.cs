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
    private readonly HreflangBuilder _hreflangBuilder;

    public SeoService(
        CanonicalUrlBuilder canonicalUrlBuilder,
        IAbsoluteUrlBuilder absoluteUrlBuilder,
        SeoPaginationBuilder seoPaginationBuilder,
        SeoIndexingPolicy seoIndexingPolicy,
        HreflangBuilder hreflangBuilder)
    {
        _canonicalUrlBuilder = canonicalUrlBuilder;
        _absoluteUrlBuilder = absoluteUrlBuilder;
        _seoPaginationBuilder = seoPaginationBuilder;
        _seoIndexingPolicy = seoIndexingPolicy;
        _hreflangBuilder = hreflangBuilder;
    }

    public PageSeoData BuildCatalogGatewaySeo(CatalogGatewayPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildCatalog(culture);
        var canonical = _absoluteUrlBuilder.Build(request, relative);

        var title = "Каталог послуг по містах — Marketplace";
        var description = "Оберіть місто, щоб перейти до локального каталогу послуг, категорій і спеціалістів.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgUrl = canonical,
            Robots = "index, follow",
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildHomePageSeo(HomePageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildHome(culture);
        var canonical = _absoluteUrlBuilder.Build(request, relative);

        var title = "Marketplace — каталог послуг у вашому місті";
        var description = "Знайдіть компанії, спеціалістів і послуги за містами, категоріями та підкатегоріями.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgType = "website",
            OgUrl = canonical,
            Robots = "index, follow",
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildCatalogIndexSeo(CatalogIndexPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildCatalog(culture, model.ListingsSection.Filter.Page);
        var canonical = _absoluteUrlBuilder.Build(request, relative);
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = "Каталог послуг — Marketplace";
        var description = "Каталог послуг за містами та категоріями. Знаходьте потрібні сервіси швидко та зручно.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination),
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildCityPageSeo(CityPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildCity(model);
        var canonical = _absoluteUrlBuilder.Build(request, relative);
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = $"Послуги у {model.CityName} — каталог компаній та спеціалістів";
        var description = $"Знайдіть послуги у {model.CityName}: компанії, спеціалісти, категорії та актуальні пропозиції.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination),
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildCategoryPageSeo(CategoryPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildCategory(model);
        var canonical = _absoluteUrlBuilder.Build(request, relative);
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = $"{model.CategoryName} у {model.CityName} — послуги та спеціалісти";
        var description = $"Перегляньте {model.CategoryName.ToLower()} у {model.CityName}: компанії, спеціалісти, контакти та актуальні пропозиції.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination),
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildSubCategoryPageSeo(SubCategoryPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildSubCategory(model);
        var canonical = _absoluteUrlBuilder.Build(request, relative);
        var robots = _seoIndexingPolicy.GetRobots(model.ListingsSection.Filter);

        var title = $"{model.SubCategoryName} у {model.CityName} — каталог послуг";
        var description = $"Знайдіть {model.SubCategoryName.ToLower()} у {model.CityName}: актуальні пропозиції, контакти та відгуки.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Hero.Title,
            SeoText = model.SeoIntro.Text,
            OgTitle = title,
            OgDescription = description,
            OgUrl = canonical,
            Robots = robots,
            PrevUrl = _seoPaginationBuilder.BuildPrev(request, model.ListingsSection.Pagination),
            NextUrl = _seoPaginationBuilder.BuildNext(request, model.ListingsSection.Pagination),
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }

    public PageSeoData BuildListingDetailsSeo(ListingDetailsPageVm model, HttpRequest request, string culture)
    {
        var relative = _canonicalUrlBuilder.BuildListing(model);
        var canonical = _absoluteUrlBuilder.Build(request, relative);

        var image = model.Gallery.Images.FirstOrDefault(x => x.IsPrimary)?.Url
                    ?? model.Gallery.Images.FirstOrDefault()?.Url;

        var title = $"{model.Title} у {model.CityName} — контакти, адреса, відгуки";
        var description = !string.IsNullOrWhiteSpace(model.ShortDescription)
            ? model.ShortDescription
            : $"Детальна інформація про {model.Title} у {model.CityName}: контакти, адреса, рейтинг та відгуки.";

        return new PageSeoData
        {
            Title = title,
            Description = description,
            CanonicalUrl = canonical,
            H1 = model.Title,
            SeoText = model.Description,
            OgTitle = title,
            OgDescription = description,
            OgImage = image is null ? null : _absoluteUrlBuilder.Build(request, image),
            OgType = "article",
            OgUrl = canonical,
            Robots = "index, follow",
            Hreflangs = _hreflangBuilder.Build(request, relative)
        };
    }
}