using Marketplace.Web.Models.Listings.Forms;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateReview(
    string culture,
    CreateListingReviewVm model,
    CancellationToken cancellationToken)
{
    culture = CultureHelper.NormalizeRouteCulture(culture);

    if (!ModelState.IsValid)
    {
        TempData["ReviewError"] = culture == "uk"
            ? "Будь ласка, перевірте форму."
            : "Please check the form.";

        return RedirectToAction("Details", new { culture, id = model.ListingId });
    }

    // var listing = await _dbContext.Listings
    //     .Include(x => x.Reviews)
    //     .FirstOrDefaultAsync(x => x.Id == model.ListingId, cancellationToken);

    // if (listing == null)
    // {
    //     return NotFound();
    // }

    // var review = new ListingReview
    // {
    //     Id = Guid.NewGuid(),
    //     Listing = listing,
    //     AuthorName = model.AuthorName,
    //     Email = model.Email,
    //     Rating = model.Rating ?? 5,
    //     Text = model.Text,
    //     IsApproved = true, // або false якщо модерація
    //     CreatedAtUtc = DateTime.UtcNow
    // };

    // listing.Reviews.Add(review);

    // // 🔥 Перерахунок рейтингу
    // listing.ReviewsCount = listing.Reviews.Count;
    // listing.Rating = Math.Round(listing.Reviews.Average(x => x.Rating), 1);

    // await _dbContext.SaveChangesAsync(cancellationToken);

    // TempData["ReviewSuccess"] = culture == "uk"
    //     ? "Дякуємо за відгук!"
    //     : "Thank you for your review!";

    return RedirectToAction("Details", new { culture, id = model.ListingId });
    }
}