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

    public HomeController(IHomeService homeService, ISeoService seoService)
    {
        _homeService = homeService;
        _seoService = seoService;
    }

    [HttpGet("/{culture:regex(^uk|en$)}")]
    public async Task<IActionResult> Index(string culture, CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        Request.Cookies.TryGetValue(PreferredCityCookieName, out var selectedCitySlug);

        var vm = await _homeService.GetHomePageAsync(culture, selectedCitySlug, cancellationToken);
        ViewData["Seo"] = _seoService.BuildHomePageSeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/privacy")]
    public IActionResult Privacy(string culture)
    {
        culture = CultureHelper.Normalize(culture);

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
}