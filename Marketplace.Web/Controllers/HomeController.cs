using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Web.Models;
using Marketplace.Web.Models.Home;
using Marketplace.Web.Seo;

namespace Marketplace.Web.Controllers;

public class HomeController : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var vm = new HomeIndexPageVm();

        this.SetSeo(new PageSeoData
            {
                // Title = $"",
                // Description = $"",
                // CanonicalUrl = Url.Action("Subcategory", "Catalog", new
                // {
                //     city = vm.CitySlug,
                //     category = vm.CategorySlug,
                //     subcategory = vm.SubCategorySlug
                // }, Request.Scheme),
                // Robots = "index,follow"
            });

        return View(vm);
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
