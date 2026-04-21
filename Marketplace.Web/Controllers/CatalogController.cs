using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Services.Seo;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class CatalogController : Controller
{
    private const string PreferredCityCookieName = "preferred_city";
    
    private readonly ICatalogService _catalogService;
    private readonly ISeoService _seoService;

    public CatalogController(
        ICatalogService catalogService,
        ISeoService seoService)
    {
        _catalogService = catalogService;
        _seoService = seoService;
    }

    [HttpGet("/{culture:regex(^uk|en$)}/catalog")]
    [HttpGet("/{culture:regex(^uk|en$)}/catalog/page-{page:int}")]
    public async Task<IActionResult> Index(
        string culture,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/catalog");
        }

        Request.Cookies.TryGetValue(PreferredCityCookieName, out var selectedCitySlug);

        var vm = await _catalogService.GetCatalogGatewayPageAsync(culture, selectedCitySlug, cancellationToken);
        ViewData["Seo"] = _seoService.BuildCatalogGatewaySeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}")]
    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/page-{page:int}")]
    public async Task<IActionResult> City(
        string culture,
        string citySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);
        filter.Page = page ?? 1;

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/{citySlug}");
        }

        var vm = await _catalogService.GetCityPageAsync(culture, citySlug, filter, cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        ViewData["Seo"] = _seoService.BuildCityPageSeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}")]
    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}/page-{page:int}")]
    public async Task<IActionResult> Category(
        string culture,
        string citySlug,
        string categorySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);
        filter.Page = page ?? 1;

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/{citySlug}/{categorySlug}");
        }

        var vm = await _catalogService.GetCategoryPageAsync(culture, citySlug, categorySlug, filter, cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        ViewData["Seo"] = _seoService.BuildCategoryPageSeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}/{subCategorySlug}")]
    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}/{subCategorySlug}/page-{page:int}")]
    public async Task<IActionResult> SubCategory(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);
        filter.Page = page ?? 1;

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/{citySlug}/{categorySlug}/{subCategorySlug}");
        }

        var vm = await _catalogService.GetSubCategoryPageAsync(
            culture,
            citySlug,
            categorySlug,
            subCategorySlug,
            filter,
            cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        ViewData["Seo"] = _seoService.BuildSubCategoryPageSeo(vm, Request, culture);

        return View(vm);
    }
}