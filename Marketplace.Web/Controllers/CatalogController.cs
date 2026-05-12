using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Navigation;
using Marketplace.Web.Seo;
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
    private readonly StructuredDataBuilder _structuredDataBuilder;
    private readonly IAbsoluteUrlBuilder _absoluteUrlBuilder;
    private readonly ICatalogUrlBuilder _urlBuilder;

    public CatalogController(
        ICatalogService catalogService,
        ISeoService seoService,
        StructuredDataBuilder structuredDataBuilder,
        IAbsoluteUrlBuilder absoluteUrlBuilder,
        ICatalogUrlBuilder urlBuilder)
    {
        _catalogService = catalogService;
        _seoService = seoService;
        _structuredDataBuilder = structuredDataBuilder;
        _absoluteUrlBuilder = absoluteUrlBuilder;
        _urlBuilder = urlBuilder;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/catalog")]
    [HttpGet("/{culture:regex(^uk|ru$)}/catalog/page-{page:int}")]
    public async Task<IActionResult> Index(
        string culture,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/catalog");
        }

        Request.Cookies.TryGetValue(PreferredCityCookieName, out var selectedCitySlug);

        var vm = await _catalogService.GetCatalogGatewayPageAsync(culture, selectedCitySlug, cancellationToken);
        ViewData["Seo"] = _seoService.BuildCatalogGatewaySeo(vm, Request, culture);

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}")]
    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/page-{page:int}")]
    public async Task<IActionResult> City(
        string culture,
        string citySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
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

        var homeUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildHomeUrl(culture));
        var cityUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCityUrl(culture, citySlug));

        ViewData["BreadcrumbsStructuredData"] = _structuredDataBuilder.BuildBreadcrumbs(
        [
            ("Marketplace", homeUrl),
            (vm.CityName, cityUrl)
        ]);

        var listingItems = vm.ListingsSection.Listings
            .Select(x => (x.Title, _absoluteUrlBuilder.Build(Request, x.Url)))
            .ToList();

        if (listingItems.Count > 0)
        {
            ViewData["StructuredData"] = _structuredDataBuilder.BuildItemList(listingItems);
        }

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/{categorySlug}")]
    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/{categorySlug}/page-{page:int}")]
    public async Task<IActionResult> Category(
        string culture,
        string citySlug,
        string categorySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
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

        var homeUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildHomeUrl(culture));
        var cityUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCityUrl(culture, citySlug));
        var categoryUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug));

        ViewData["BreadcrumbsStructuredData"] = _structuredDataBuilder.BuildBreadcrumbs(
        [
            ("Marketplace", homeUrl),
            (vm.CityName, cityUrl),
            (vm.CategoryName, categoryUrl)
        ]);

        var listingItems = vm.ListingsSection.Listings
            .Select(x => (x.Title, _absoluteUrlBuilder.Build(Request, x.Url)))
            .ToList();

        if (listingItems.Count > 0)
        {
            ViewData["StructuredData"] = _structuredDataBuilder.BuildItemList(listingItems);
        }

        return View(vm);
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/{categorySlug}/{subCategorySlug}")]
    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/{categorySlug}/{subCategorySlug}/page-{page:int}")]
    public async Task<IActionResult> SubCategory(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        int? page,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
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

        var homeUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildHomeUrl(culture));
        var cityUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCityUrl(culture, citySlug));
        var categoryUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildCategoryUrl(culture, citySlug, categorySlug));
        var subCategoryUrl = _absoluteUrlBuilder.Build(Request, _urlBuilder.BuildSubCategoryUrl(culture, citySlug, categorySlug, subCategorySlug));

        ViewData["BreadcrumbsStructuredData"] = _structuredDataBuilder.BuildBreadcrumbs(
        [
            ("Marketplace", homeUrl),
            (vm.CityName ?? citySlug, cityUrl),
            (vm.CategoryName ?? categorySlug, categoryUrl),
            (vm.SubCategoryName, subCategoryUrl)
        ]);

        var listingItems = vm.ListingsSection.Listings
            .Select(x => (x.Title, _absoluteUrlBuilder.Build(Request, x.Url)))
            .ToList();

        if (listingItems.Count > 0)
        {
            ViewData["StructuredData"] = _structuredDataBuilder.BuildItemList(listingItems);
        }

        return View(vm);
    }
}
