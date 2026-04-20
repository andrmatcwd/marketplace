using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

[Route("error")]
public sealed class ErrorController : Controller
{
    [Route("404")]
    public IActionResult NotFoundPage()
    {
        Response.StatusCode = 404;
        return View("NotFound");
    }

    [Route("500")]
    public IActionResult ServerError()
    {
        Response.StatusCode = 500;
        return View("ServerError");
    }
}