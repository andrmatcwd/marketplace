using Marketplace.Web.Models.Services;
using Marketplace.Web.Services.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("api/services")]
public sealed class ServicesApiController(IServiceCatalogService catalogService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ServiceItemViewModel>>> Get(
        [FromQuery] ServicesFilterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await catalogService.GetServicesAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ServiceCategoryViewModel>>> GetCategories(
        CancellationToken cancellationToken)
    {
        var categories = await catalogService.GetCategoriesAsync(cancellationToken);
        return Ok(categories);
    }
}