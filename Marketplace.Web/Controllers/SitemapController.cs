using System.Text;
using Marketplace.Modules.Listings.Application.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class SitemapController : Controller
{
    private readonly IMediator _mediator;

    public SitemapController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/sitemap.xml")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var data = await _mediator.Send(new GetSitemapDataQuery(), cancellationToken);

        var urls = new List<string>
        {
            $"{baseUrl}/uk",
            $"{baseUrl}/ru",
            $"{baseUrl}/uk/catalog",
            $"{baseUrl}/ru/catalog"
        };

        foreach (var city in data.Cities)
        {
            urls.Add($"{baseUrl}/uk/{city.Slug}");
            urls.Add($"{baseUrl}/ru/{city.Slug}");
        }

        foreach (var city in data.Cities)
        {
            foreach (var cat in data.Categories)
            {
                urls.Add($"{baseUrl}/uk/{city.Slug}/{cat.Slug}");
                urls.Add($"{baseUrl}/ru/{city.Slug}/{cat.Slug}");
            }
        }

        foreach (var city in data.Cities)
        {
            foreach (var sub in data.SubCategories)
            {
                urls.Add($"{baseUrl}/uk/{city.Slug}/{sub.CategorySlug}/{sub.Slug}");
                urls.Add($"{baseUrl}/ru/{city.Slug}/{sub.CategorySlug}/{sub.Slug}");
            }
        }

        foreach (var listing in data.Listings)
        {
            urls.Add($"{baseUrl}/uk/{listing.CitySlug}/{listing.CategorySlug}/{listing.SubCategorySlug}/{listing.Slug}/{listing.Id}");
            urls.Add($"{baseUrl}/ru/{listing.CitySlug}/{listing.CategorySlug}/{listing.SubCategorySlug}/{listing.Slug}/{listing.Id}");
        }

        var sb = new StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");

        foreach (var url in urls.Distinct())
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(url)}</loc>");
            sb.AppendLine("  </url>");
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }
}
