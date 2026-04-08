using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Web.Models;
using Marketplace.Web.Services.Listing;
using Marketplace.Web.Models.Listings;

namespace Marketplace.Web.Controllers;

public class HomeController(IListingCatalogService catalogService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await catalogService.GetListingsAsync(new ListingsFilterRequest
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
