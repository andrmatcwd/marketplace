using Marketplace.Web.Models.Common;
using Marketplace.Web.Seo;
using Marketplace.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController(IListingService listingService) : Controller
{
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