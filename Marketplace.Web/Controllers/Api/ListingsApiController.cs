using Marketplace.Web.Models.Category;
using Marketplace.Web.Models.Common;
using Marketplace.Web.Models.Listings;
using Marketplace.Web.Services.Listing;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("api/listings")]
public sealed class ListingsApiController(IListingCatalogService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ListingViewModel>>> Get(
        [FromQuery] ListingsFilterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetListingsAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryViewModel>>> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await catalogService.GetCategoriesAsync(cancellationToken);
        return Ok(categories);
    }
}