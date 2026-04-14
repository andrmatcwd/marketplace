using Marketplace.Web.Models.Catalog;
using Marketplace.Web.Services.Catalog;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/listings")]
public sealed class ListingsApiController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public ListingsApiController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
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
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.Normalize(culture);

        if (string.IsNullOrWhiteSpace(search))
        {
            return Ok(Array.Empty<object>());
        }

        var result = await _catalogService.GetCatalogIndexPageAsync(
            culture,
            new CatalogFilterVm
            {
                Search = search,
                Page = 1,
                PageSize = 8
            },
            cancellationToken);

        var payload = result.ListingsSection.Listings
            .Take(8)
            .Select(x => new
            {
                label = x.Title,
                url = x.Url,
                city = x.CityName
            });

        return Ok(payload);
    }
}