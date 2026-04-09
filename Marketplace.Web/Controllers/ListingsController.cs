using Marketplace.Web.Models.Listings;
using Marketplace.Web.Services.Listing;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ListingsController(IListingCatalogService catalogService) : Controller
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
}