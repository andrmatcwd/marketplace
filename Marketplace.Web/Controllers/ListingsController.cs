using Marketplace.Web.Navigation;
using Marketplace.Web.Seo;
using Marketplace.Web.Services.Listings;
using Marketplace.Web.Services.Seo;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController : Controller
{
    private readonly IListingService _listingService;
    private readonly ISeoService _seoService;
    private readonly ICatalogUrlBuilder _urlBuilder;
    private readonly IAbsoluteUrlBuilder _absoluteUrlBuilder;
    private readonly StructuredDataBuilder _structuredDataBuilder;

    public ListingsController(
        IListingService listingService,
        ISeoService seoService,
        ICatalogUrlBuilder urlBuilder,
        IAbsoluteUrlBuilder absoluteUrlBuilder,
        StructuredDataBuilder structuredDataBuilder)
    {
        _listingService = listingService;
        _seoService = seoService;
        _urlBuilder = urlBuilder;
        _absoluteUrlBuilder = absoluteUrlBuilder;
        _structuredDataBuilder = structuredDataBuilder;
    }

    [HttpGet("/{culture:regex(^uk|en$)}/{citySlug}/{categorySlug}/{subCategorySlug}/{serviceSlug}/{id:guid}")]
    public async Task<IActionResult> Details(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        Guid id,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        var vm = await _listingService.GetDetailsPageAsync(
            culture,
            citySlug,
            categorySlug,
            subCategorySlug,
            serviceSlug,
            id,
            cancellationToken);

        if (vm is null)
        {
            return NotFound();
        }

        var canonicalPath = _urlBuilder.BuildListingUrl(
            culture,
            vm.CitySlug!,
            vm.CategorySlug!,
            vm.SubCategorySlug!,
            vm.Slug,
            vm.Id);

        var requestedPath = Request.Path.Value ?? string.Empty;

        if (!string.Equals(requestedPath, canonicalPath, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectPermanent(canonicalPath);
        }

        ViewData["Seo"] = _seoService.BuildListingDetailsSeo(vm, Request, culture);

        var absoluteCanonical = _absoluteUrlBuilder.Build(Request, canonicalPath);
        var primaryImage = vm.Gallery.Images.FirstOrDefault(x => x.IsPrimary)?.Url
                           ?? vm.Gallery.Images.FirstOrDefault()?.Url;

        var absoluteImage = string.IsNullOrWhiteSpace(primaryImage)
            ? null
            : _absoluteUrlBuilder.Build(Request, primaryImage);

        ViewData["StructuredData"] = _structuredDataBuilder.BuildListing(vm, absoluteCanonical, absoluteImage);

        var breadcrumbItems = vm.Breadcrumbs
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .Select(x => (x.Title, _absoluteUrlBuilder.Build(Request, x.Url!)))
            .ToList();

        if (breadcrumbItems.Count > 0)
        {
            ViewData["BreadcrumbsStructuredData"] = _structuredDataBuilder.BuildBreadcrumbs(breadcrumbItems);
        }

        return View("Details", vm);
    }
}