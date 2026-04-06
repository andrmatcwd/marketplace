using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Web.Models;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Models.Services;

namespace Marketplace.Web.Controllers;

public class HomeController(IServiceCatalogService catalogService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetServicesAsync(new ServicesFilterRequest
        {
            Page = 1,
            PageSize = 6,
            SortBy = "rating_desc"
        }, cancellationToken);

        return View(result.Items);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
