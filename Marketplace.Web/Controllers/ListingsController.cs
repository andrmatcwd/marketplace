using System.Security.Claims;
using Marketplace.Web.Models.Listings.Forms;
using Marketplace.Web.Navigation;
using Marketplace.Web.Seo;
using Marketplace.Web.Services.Listings;
using Marketplace.Web.Services.Seo;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("/{culture:regex(^uk|ru$)}/{citySlug}/{categorySlug}/{subCategorySlug}/{serviceSlug}/{id:int}")]
    public async Task<IActionResult> Details(
        string culture,
        string citySlug,
        string categorySlug,
        string subCategorySlug,
        string serviceSlug,
        int id,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

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

    [HttpPost("/{culture:regex(^uk|ru$)}/review")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReview(
        string culture,
        CreateListingReviewVm model,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { success = false, message = "Unauthorized" });

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(new { success = false, errors });
        }

        await _listingService.SubmitReviewAsync(
            model.ListingId,
            userId,
            model.AuthorName,
            model.Text,
            model.Rating ?? 5,
            cancellationToken);

        return Ok(new { success = true });
    }
}