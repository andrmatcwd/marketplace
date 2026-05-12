using Marketplace.Web.Navigation;
using Marketplace.Web.Seo;
using Marketplace.Web.Services.Home;
using Marketplace.Web.Services.Seo;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class HomeController : Controller
{
    private const string PreferredCityCookieName = "preferred_city";

    private readonly IHomeService _homeService;
    private readonly ISeoService _seoService;
    private readonly StructuredDataBuilder _structuredDataBuilder;
    private readonly IAbsoluteUrlBuilder _absoluteUrlBuilder;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public HomeController(
        IHomeService homeService,
        ISeoService seoService,
        StructuredDataBuilder structuredDataBuilder,
        IAbsoluteUrlBuilder absoluteUrlBuilder,
        ICatalogUrlBuilder urlBuilder)
    {
        _homeService = homeService;
        _seoService = seoService;
        _structuredDataBuilder = structuredDataBuilder;
        _absoluteUrlBuilder = absoluteUrlBuilder;
        _urlBuilder = urlBuilder;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}")]
    public async Task<IActionResult> Index(string culture, CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        Request.Cookies.TryGetValue(PreferredCityCookieName, out var selectedCitySlug);

        var vm = await _homeService.GetHomePageAsync(culture, selectedCitySlug, cancellationToken);
        ViewData["Seo"] = _seoService.BuildHomePageSeo(vm, Request, culture);

        var siteUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildHomeUrl(culture));
        var searchUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCatalogUrl(culture)) + "?search=";
        ViewData["StructuredData"] = _structuredDataBuilder.BuildWebSite(siteUrl, searchUrl);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/privacy")]
    public IActionResult Privacy(string culture)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        ViewData["Seo"] = new PageSeoData
        {
            Title = culture == "uk" ? "Політика конфіденційності" : "Privacy Policy",
            Description = culture == "uk"
                ? "Інформація про конфіденційність і обробку даних."
                : "Information about privacy and data processing.",
            Robots = "index, follow"
        };

        return View();
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/articles")]
    public IActionResult Articles(string culture)
    {
        return View();
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/about")]
    public IActionResult About(string culture)
    {
        return View();
    }
}