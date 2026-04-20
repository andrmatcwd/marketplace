using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Services.Seo;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISeoService _seoService;

    public CatalogController(ICatalogService catalogService, ISeoService seoService)
    {
        _catalogService = catalogService;
        _seoService = seoService;
    }

    [HttpGet("/{culture:regex(^uk|en$)}/catalog")]
    public async Task<IActionResult> Index(
        string culture,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        var vm = await _catalogService.GetCatalogIndexPageAsync(culture, filter, cancellationToken);
        ViewData["Seo"] = _seoService.BuildCatalogIndexSeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}")]
    public async Task<IActionResult> City(
        string culture,
        string citySlug,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        var vm = await _catalogService.GetCityPageAsync(culture, citySlug, filter, cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        ViewData["Seo"] = _seoService.BuildCityPageSeo(vm, Request, culture);
        return View("City", vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}")]
    public async Task<IActionResult> Category(
        string culture,
        string citySlug,
        string categorySlug,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        var vm = await _catalogService.GetCategoryPageAsync(culture, citySlug, categorySlug, filter, cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        ViewData["Seo"] = _seoService.BuildCategoryPageSeo(vm, Request, culture);
        return View("Category", vm);
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}/{subCategorySlug}")]
    public async Task<IActionResult> SubCategory(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

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
        return View("SubCategory", vm);
    }
}