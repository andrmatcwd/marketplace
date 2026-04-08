using Marketplace.Web.Areas.Admin.Models.Listings;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Services.Listing;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public sealed class ListingsController(IListingCatalogService catalogService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery] AdminListingsFilterViewModel filter,
            CancellationToken cancellationToken)
        {
            var categories = await catalogService.GetCategoriesAsync(cancellationToken);

            var result = await catalogService.GetListingsAsync(
                new ListingsFilterRequest
                {
                    Search = filter.Search,
                    Page = filter.Page <= 0 ? 1 : filter.Page,
                    PageSize = filter.PageSize <= 0 ? 10 : filter.PageSize,
                    PriceFrom = filter.PriceFrom,
                    PriceTo = filter.PriceTo,
                    OnlineOnly = filter.OnlineOnly,
                    OfflineOnly = filter.OfflineOnly,
                    Categories = string.IsNullOrWhiteSpace(filter.Category)
                        ? []
                        : [filter.Category],
                    SortBy = "title_asc"
                },
                cancellationToken);

            var items = result.Items.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                items = items.Where(x =>
                    !string.IsNullOrWhiteSpace(x.City) &&
                    x.City.Contains(filter.City, StringComparison.OrdinalIgnoreCase));
            }

            var finalItems = items.ToList();

            var model = new AdminListingsPageViewModel
            {
                Filter = new AdminListingsFilterViewModel
                {
                    Search = filter.Search,
                    Category = filter.Category,
                    City = filter.City,
                    PriceFrom = filter.PriceFrom,
                    PriceTo = filter.PriceTo,
                    OnlineOnly = filter.OnlineOnly,
                    OfflineOnly = filter.OfflineOnly,
                    Page = result.Page,
                    PageSize = result.PageSize,
                    Categories = categories
                },
                Items = finalItems,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            ViewBag.Categories = await catalogService.GetCategoriesAsync(cancellationToken);

            return View(new ListingItemViewModel
            {
                Currency = "USD",
                IsOffline = true
            });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingItemViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await catalogService.GetCategoriesAsync(cancellationToken);
                return View(model);
            }

            await catalogService.CreateAsync(model, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var service = await catalogService.GetByIdAsync(id, cancellationToken);
            if (service is null)
                return NotFound();

            ViewBag.Categories = await catalogService.GetCategoriesAsync(cancellationToken);
            return View(service);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ListingItemViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await catalogService.GetCategoriesAsync(cancellationToken);
                return View(model);
            }

            await catalogService.UpdateAsync(model, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await catalogService.DeleteAsync(id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

    }
}
