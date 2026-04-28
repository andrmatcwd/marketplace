using System.Text;
using Marketplace.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Controllers;

public sealed class SitemapController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public SitemapController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("/sitemap.xml")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var urls = new List<string>
        {
            $"{baseUrl}/uk",
            $"{baseUrl}/ru",
            $"{baseUrl}/uk/catalog",
            $"{baseUrl}/ru/catalog"
        };

        var cities = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .ToListAsync(cancellationToken);

        foreach (var city in cities)
        {
            urls.Add($"{baseUrl}/uk/{city.Slug}");
            urls.Add($"{baseUrl}/ru/{city.Slug}");
        }

        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(x => x.IsPublished)
            .ToListAsync(cancellationToken);

        foreach (var city in cities)
        {
            foreach (var category in categories)
            {
                urls.Add($"{baseUrl}/uk/{city.Slug}/{category.Slug}");
                urls.Add($"{baseUrl}/ru/{city.Slug}/{category.Slug}");
            }
        }

        var subCategories = await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.IsPublished)
            .ToListAsync(cancellationToken);

        foreach (var city in cities)
        {
            foreach (var subCategory in subCategories.Where(x => x.Category != null))
            {
                urls.Add($"{baseUrl}/uk/{city.Slug}/{subCategory.Category!.Slug}/{subCategory.Slug}");
                urls.Add($"{baseUrl}/ru/{city.Slug}/{subCategory.Category!.Slug}/{subCategory.Slug}");
            }
        }

        var listings = await _dbContext.Listings
            .AsNoTracking()
            .Include(x => x.City)
            .Include(x => x.Category)
            .Include(x => x.SubCategory)
            .Where(x => x.IsPublished && x.City != null && x.Category != null && x.SubCategory != null)
            .ToListAsync(cancellationToken);

        foreach (var listing in listings)
        {
            urls.Add($"{baseUrl}/uk/{listing.City!.Slug}/{listing.Category!.Slug}/{listing.SubCategory!.Slug}/{listing.Slug}-{listing.Id}");
            urls.Add($"{baseUrl}/ru/{listing.City!.Slug}/{listing.Category!.Slug}/{listing.SubCategory!.Slug}/{listing.Slug}-{listing.Id}");
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