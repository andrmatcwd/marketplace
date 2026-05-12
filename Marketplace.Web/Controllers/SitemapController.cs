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
        var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

        var sb = new StringBuilder();
        sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
        sb.AppendLine("""<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">""");

        // Home pages
        foreach (var lang in new[] { "uk", "ru" })
        {
            AppendUrl(sb, $"{baseUrl}/{lang}", today, "weekly", "1.0");
            AppendUrl(sb, $"{baseUrl}/{lang}/catalog", today, "daily", "0.9");
        }

        // City pages
        foreach (var city in data.Cities)
        {
            foreach (var lang in new[] { "uk", "ru" })
            {
                AppendUrl(sb, $"{baseUrl}/{lang}/{city.Slug}", today, "daily", "0.8");
            }
        }

        // Category pages
        foreach (var city in data.Cities)
        {
            foreach (var cat in data.Categories)
            {
                foreach (var lang in new[] { "uk", "ru" })
                {
                    AppendUrl(sb, $"{baseUrl}/{lang}/{city.Slug}/{cat.Slug}", today, "weekly", "0.7");
                }
            }
        }

        // SubCategory pages
        foreach (var city in data.Cities)
        {
            foreach (var sub in data.SubCategories)
            {
                foreach (var lang in new[] { "uk", "ru" })
                {
                    AppendUrl(sb, $"{baseUrl}/{lang}/{city.Slug}/{sub.CategorySlug}/{sub.Slug}", today, "weekly", "0.6");
                }
            }
        }

        // Listing detail pages
        foreach (var listing in data.Listings)
        {
            var lastmod = listing.UpdatedAtUtc.ToString("yyyy-MM-dd");
            foreach (var lang in new[] { "uk", "ru" })
            {
                var url = $"{baseUrl}/{lang}/{listing.CitySlug}/{listing.CategorySlug}/{listing.SubCategorySlug}/{listing.Slug}/{listing.Id}";
                AppendUrl(sb, url, lastmod, "monthly", "0.8");
            }
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    private static void AppendUrl(StringBuilder sb, string loc, string lastmod, string changefreq, string priority)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(loc)}</loc>");
        sb.AppendLine($"    <lastmod>{lastmod}</lastmod>");
        sb.AppendLine($"    <changefreq>{changefreq}</changefreq>");
        sb.AppendLine($"    <priority>{priority}</priority>");
        sb.AppendLine("  </url>");
    }
}
