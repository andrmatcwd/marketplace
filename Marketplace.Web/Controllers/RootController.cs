using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class RootController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return RedirectPermanent("/uk");
    }
}