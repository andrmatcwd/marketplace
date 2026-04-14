using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class RobotsController : Controller
{
    [HttpGet("/robots.txt")]
    public IActionResult Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var content = $"""
User-agent: *
Disallow: /error/
Disallow: /admin/
Sitemap: {baseUrl}/sitemap.xml
""";

        return Content(content, "text/plain");
    }
}