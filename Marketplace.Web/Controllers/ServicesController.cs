using Marketplace.Web.Models.Services;
using Marketplace.Web.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class ServicesController(IServiceCatalogService catalogService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ServicesFilterRequest request, CancellationToken cancellationToken)
    {
        var model = new ServicesPageViewModel
        {
            Filters = request,
            Categories = await catalogService.GetCategoriesAsync(cancellationToken),
            Results = await catalogService.GetServicesAsync(request, cancellationToken)
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ServicesFilterRequest request, CancellationToken cancellationToken)
    {
        var model = new ServicesPageViewModel
        {
            Filters = request,
            Categories = await catalogService.GetCategoriesAsync(cancellationToken),
            Results = await catalogService.GetServicesAsync(request, cancellationToken)
        };

        return PartialView("_ServicesGrid", model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var service = (await catalogService.GetServicesAsync(new ServicesFilterRequest
        {
            PageSize = int.MaxValue
        }, cancellationToken))
        .Items.FirstOrDefault(x => x.Id == id);

        if (service == null)
            return NotFound();

        return View(service);
    }
}