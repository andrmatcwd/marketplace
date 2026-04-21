using Marketplace.Web.Data;
using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/listings")]
public sealed class ListingsApiController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly ApplicationDbContext _dbContext;

    public ListingsApiController(
        ICatalogService catalogService,
        ApplicationDbContext dbContext)
    {
        _catalogService = catalogService;
        _dbContext = dbContext;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        string culture,
        [FromQuery] CatalogFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        var result = await _catalogService.GetCatalogIndexPageAsync(culture, filter, cancellationToken);

        var payload = result.ListingsSection.Listings.Select(x => new
        {
            x.Id,
            x.Title,
            x.Url,
            x.CityName,
            x.CategoryName,
            x.SubCategoryName,
            x.ShortDescription,
            x.Rating,
            x.ReviewsCount
        });

        return Ok(payload);
    }

    [HttpGet("autocomplete")]
    public async Task<IActionResult> AutoComplete(
        string culture,
        [FromQuery] string? search,
        [FromQuery] string? city,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        if (string.IsNullOrWhiteSpace(search))
        {
            return Ok(Array.Empty<object>());
        }

        var trimmedSearch = search.Trim();
        var trimmedCity = city?.Trim();

        if (!string.IsNullOrWhiteSpace(trimmedCity))
        {
            var cityId = await _dbContext.Cities
                .AsNoTracking()
                .Where(x => x.IsPublished && x.Slug == trimmedCity)
                .Select(x => (Guid?)x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (cityId.HasValue)
            {
                var normalized = trimmedSearch.ToLower();

                var subCategorySuggestions = await _dbContext.SubCategories
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Where(x =>
                        x.IsPublished &&
                        x.Category != null &&
                        x.Category.IsPublished &&
                        x.Listings.Any(l => l.CityId == cityId.Value && l.IsPublished) &&
                        (EF.Functions.Like(x.Name.ToLower(), $"%{normalized}%") ||
                         EF.Functions.Like(x.Slug.ToLower(), $"%{normalized}%")))
                    .Select(x => new
                    {
                        type = "subcategory",
                        label = x.Name,
                        url = "/" + culture + "/" + trimmedCity + "/" + x.Category!.Slug + "/" + x.Slug,
                        city = trimmedCity,
                        category = x.Category!.Name,
                        score =
                            x.Name.ToLower() == normalized || x.Slug.ToLower() == normalized ? 300 :
                            x.Name.ToLower().StartsWith(normalized) || x.Slug.ToLower().StartsWith(normalized) ? 200 :
                            100,
                        listingsCount = x.Listings.Count(l => l.CityId == cityId.Value && l.IsPublished)
                    })
                    .OrderByDescending(x => x.score)
                    .ThenByDescending(x => x.listingsCount)
                    .ThenBy(x => x.label)
                    .Take(5)
                    .ToListAsync(cancellationToken);

                var listingResult = await _catalogService.GetCatalogIndexPageAsync(
                    culture,
                    new CatalogFilterVm
                    {
                        Search = trimmedSearch,
                        City = trimmedCity,
                        Page = 1,
                        PageSize = 5
                    },
                    cancellationToken);

                var listingSuggestions = listingResult.ListingsSection.Listings
                    .Select(x => new
                    {
                        type = "listing",
                        label = x.Title,
                        url = x.Url,
                        city = x.CityName,
                        category = x.SubCategoryName ?? x.CategoryName
                    })
                    .Take(5)
                    .ToList();

                var payload = subCategorySuggestions
                    .Cast<object>()
                    .Concat(listingSuggestions)
                    .Take(8);

                return Ok(payload);
            }
        }

        var fallbackResult = await _catalogService.GetCatalogIndexPageAsync(
            culture,
            new CatalogFilterVm
            {
                Search = trimmedSearch,
                Page = 1,
                PageSize = 8
            },
            cancellationToken);

        var fallbackPayload = fallbackResult.ListingsSection.Listings
            .Select(x => new
            {
                type = "listing",
                label = x.Title,
                url = x.Url,
                city = x.CityName,
                category = x.SubCategoryName ?? x.CategoryName
            })
            .Take(8);

        return Ok(fallbackPayload);
    }

    [HttpGet("resolve-route")]
    public async Task<IActionResult> ResolveRoute(
        string culture,
        [FromQuery] string? city,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(search))
        {
            return Ok(new
            {
                canRouteDirect = false
            });
        }

        var trimmedCity = city.Trim();
        var trimmedSearch = search.Trim();
        var normalized = trimmedSearch.ToLower();

        var cityId = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Slug == trimmedCity)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (!cityId.HasValue)
        {
            return Ok(new
            {
                canRouteDirect = false
            });
        }

        var match = await _dbContext.SubCategories
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x =>
                x.IsPublished &&
                x.Category != null &&
                x.Category.IsPublished &&
                x.Listings.Any(l => l.CityId == cityId.Value && l.IsPublished) &&
                (EF.Functions.Like(x.Name.ToLower(), $"%{normalized}%") ||
                 EF.Functions.Like(x.Slug.ToLower(), $"%{normalized}%")))
            .Select(x => new
            {
                x.Name,
                x.Slug,
                CategoryName = x.Category!.Name,
                CategorySlug = x.Category!.Slug,
                ListingsCount = x.Listings.Count(l => l.CityId == cityId.Value && l.IsPublished),
                Score =
                    x.Name.ToLower() == normalized || x.Slug.ToLower() == normalized ? 300 :
                    x.Name.ToLower().StartsWith(normalized) || x.Slug.ToLower().StartsWith(normalized) ? 200 :
                    x.Name.ToLower().Contains(normalized) || x.Slug.ToLower().Contains(normalized) ? 100 :
                    0
            })
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.ListingsCount)
            .ThenBy(x => x.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (match is null || match.Score <= 0)
        {
            return Ok(new
            {
                canRouteDirect = false
            });
        }

        return Ok(new
        {
            canRouteDirect = true,
            type = "subcategory",
            url = "/" + culture + "/" + trimmedCity + "/" + match.CategorySlug + "/" + match.Slug,
            categoryName = match.CategoryName,
            subCategoryName = match.Name
        });
    }
}