using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Marketplace.Web.Services;
using Marketplace.Web.Services.Listing;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController(IListingCatalogService catalogService, IListingService listingService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListingsFilterRequest request, CancellationToken cancellationToken)
    {
        var model = new ListingsPageViewModel
        {
            Filters = request,
            Categories = await catalogService.GetCategoriesAsync(cancellationToken),
            Results = await catalogService.GetListingsAsync(request, cancellationToken)
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListingsFilterRequest request, CancellationToken cancellationToken)
    {
        var model = new ListingsPageViewModel
        {
            Filters = request,
            Categories = await catalogService.GetCategoriesAsync(cancellationToken),
            Results = await catalogService.GetListingsAsync(request, cancellationToken)
        };

        return PartialView("_ListingsGrid", model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var service = await catalogService.GetByIdAsync(id, cancellationToken);

        if (service is null)
            return NotFound();

        return View(service);
    }

    public async Task<IActionResult> Details(
        string city,
        string category,
        string subcategory,
        string slug,
        int id,
        [FromQuery] BaseFilter filter,
        CancellationToken cancellationToken)
    {
        var vm = await listingService.GetListingDetailsPageAsync(
            city, category, subcategory, slug, id, filter, cancellationToken);

        if (vm is null)
            return NotFound();

        var invalidUrl =
            !string.Equals(vm.CitySlug, city, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(vm.CategorySlug, category, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(vm.SubcategorySlug, subcategory, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(vm.ListingSlug, slug, StringComparison.OrdinalIgnoreCase);

        if (invalidUrl)
        {
            return RedirectToRoutePermanent("listing-details", new
            {
                city = vm.CitySlug,
                category = vm.CategorySlug,
                subcategory = vm.SubcategorySlug,
                slug = vm.ListingSlug,
                id = vm.Id
            });
        }

        this.SetSeo(new PageSeoData
        {
            Title = $"{vm.Title} — {vm.SubcategoryName} у {vm.CityName}",
            Description = vm.Description,
            CanonicalUrl = Url.RouteUrl("listing-details", new
            {
                city = vm.CitySlug,
                category = vm.CategorySlug,
                subcategory = vm.SubcategorySlug,
                slug = vm.ListingSlug,
                id = vm.Id
            }, Request.Scheme),
            Robots = "index,follow"
        });

        return View(vm);
    }
}