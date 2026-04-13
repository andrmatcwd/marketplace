using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Marketplace.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController : Controller
{
    private readonly IListingService _listingService;

    public ListingsController(IListingService listingService)
    {
        _listingService = listingService;
    }

    // [HttpGet]
    // public async Task<IActionResult> Index([FromQuery] ListingsFilter request, CancellationToken cancellationToken)
    // {
    //     this.SetSeo(new PageSeoData
    //         {
    //             Title = "Каталог послуг",
    //             Description = "Знайдіть послуги за містом, категорією та підкатегорією.",
    //             CanonicalUrl = Url.Action("Index", "Catalog", null, Request.Scheme),
    //             Robots = "index,follow"
    //         });

    //     var model = new ListingDetailsPageVm
    //     {
            
    //     };

    //     return View(model);
    // }

    // [HttpGet]
    // public async Task<IActionResult> List([FromQuery] ListingsFilterRequest request, CancellationToken cancellationToken)
    // {
        
    //     var model = new ListingsPageViewModel
    //     {
    //         Filters = request,
    //         Categories = new List<CategoryViewModel>()
    //         {
    //             new CategoryViewModel { Value = "category-1", Label = "Категорія 1" },
    //             new CategoryViewModel { Value = "category-2", Label = "Категорія 2" },
    //             new CategoryViewModel { Value = "category-3", Label = "Категорія 3" }
    //         },
    //         Results = new PagedResult<ListingViewModel>
    //         {
    //             Items = new List<ListingViewModel>(),
    //             Page = request.Page,
    //             PageSize = request.PageSize,
    //             TotalItems = 0,
    //             TotalPages = 0
    //         }
    //     };

    //     return PartialView("_ListingsGrid", model);
    // }

    [HttpGet]
    public async Task<IActionResult> Details(
        string citySlag,
        string categorySlag,
        string subCategorySlag,
        string listingSlug,
        int id,
        CancellationToken cancellationToken)
    {

        var listing = await _listingService.GetListingDetailsPageAsync(
            citySlag,
            categorySlag,
            subCategorySlag,
            listingSlug,
            id,
            cancellationToken
        );

        if (listing is null)
            return NotFound();

        this.SetSeo(new PageSeoData
        {
            Title = $"{listing.Title} — {listing.SubCategoryName} у {listing.CityName}",
            Description = listing.Description,
            CanonicalUrl = Url.RouteUrl("listing-details", new
            {
                city = listing.CitySlug,
                category = listing.CategorySlug,
                subCategory = listing.SubCategorySlug,
                slug = listing.ListingSlug,
                id = listing.Id
            }, Request.Scheme),
            Robots = "index,follow"
        });

        return View(listing);
    }

    // public async Task<IActionResult> Details(
    //     string city,
    //     string category,
    //     string subcategory,
    //     string slug,
    //     int id,
    //     [FromQuery] BaseFilter filter,
    //     CancellationToken cancellationToken)
    // {
    //     var vm = await listingService.GetListingDetailsPageAsync(
    //         city, category, subcategory, slug, id, filter, cancellationToken);

    //     if (vm is null)
    //         return NotFound();

    //     var invalidUrl =
    //         !string.Equals(vm.CitySlug, city, StringComparison.OrdinalIgnoreCase) ||
    //         !string.Equals(vm.CategorySlug, category, StringComparison.OrdinalIgnoreCase) ||
    //         !string.Equals(vm.SubcategorySlug, subcategory, StringComparison.OrdinalIgnoreCase) ||
    //         !string.Equals(vm.ListingSlug, slug, StringComparison.OrdinalIgnoreCase);

    //     if (invalidUrl)
    //     {
    //         return RedirectToRoutePermanent("listing-details", new
    //         {
    //             city = vm.CitySlug,
    //             category = vm.CategorySlug,
    //             subcategory = vm.SubcategorySlug,
    //             slug = vm.ListingSlug,
    //             id = vm.Id
    //         });
    //     }

    //     this.SetSeo(new PageSeoData
    //     {
    //         Title = $"{vm.Title} — {vm.SubcategoryName} у {vm.CityName}",
    //         Description = vm.Description,
    //         CanonicalUrl = Url.RouteUrl("listing-details", new
    //         {
    //             city = vm.CitySlug,
    //             category = vm.CategorySlug,
    //             subcategory = vm.SubcategorySlug,
    //             slug = vm.ListingSlug,
    //             id = vm.Id
    //         }, Request.Scheme),
    //         Robots = "index,follow"
    //     });

    //     return View(vm);
    // }
}