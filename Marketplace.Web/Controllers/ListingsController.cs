using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Seo;
using Marketplace.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListingsFilterRequest request, CancellationToken cancellationToken)
    {
        this.SetSeo(new PageSeoData
            {
                Title = "Каталог послуг",
                Description = "Знайдіть послуги за містом, категорією та підкатегорією.",
                CanonicalUrl = Url.Action("Index", "Catalog", null, Request.Scheme),
                Robots = "index,follow"
            });

        var model = new ListingsPageViewModel
        {
            Filters = request,
            Categories = new List<CategoryViewModel>()
            {
                new CategoryViewModel { Value = "category-1", Label = "Категорія 1" },
                new CategoryViewModel { Value = "category-2", Label = "Категорія 2" },
                new CategoryViewModel { Value = "category-3", Label = "Категорія 3" }
            },
            Results = new PagedResult<ListingViewModel>
            {
                Items = new List<ListingViewModel>(),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = 0,
                TotalPages = 0
            }
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ListingsFilterRequest request, CancellationToken cancellationToken)
    {
        
        var model = new ListingsPageViewModel
        {
            Filters = request,
            Categories = new List<CategoryViewModel>()
            {
                new CategoryViewModel { Value = "category-1", Label = "Категорія 1" },
                new CategoryViewModel { Value = "category-2", Label = "Категорія 2" },
                new CategoryViewModel { Value = "category-3", Label = "Категорія 3" }
            },
            Results = new PagedResult<ListingViewModel>
            {
                Items = new List<ListingViewModel>(),
                Page = request.Page,
                PageSize = request.PageSize,
                TotalItems = 0,
                TotalPages = 0
            }
        };

        return PartialView("_ListingsGrid", model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var service = new ListingDetailsPageVm
        {
            Id = id,
            Title = $"Listing {id}",
            Description = $"Description for listing {id}",
            CityName = "City",
            CitySlug = "city",
            CategoryName = "Category",
            CategorySlug = "category",
            SubcategoryName = "Subcategory",
            SubcategorySlug = "subcategory",
            ListingSlug = $"listing-{id}",
            Images = new List<ListingImageVm>(),
            Reviews = new List<ListingReviewVm>(),
            RelatedListings = new List<ListingCardVm>()
        };

        if (service is null)
            return NotFound();

        return View(service);
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