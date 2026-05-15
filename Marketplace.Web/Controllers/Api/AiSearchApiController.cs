using Marketplace.Web.Services.AiSearch;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|ru$)}/api/ai-search")]
public sealed class AiSearchApiController : ControllerBase
{
    private readonly IAiSearchService _aiSearchService;

    public AiSearchApiController(IAiSearchService aiSearchService)
    {
        _aiSearchService = aiSearchService;
    }

    [HttpPost]
    public async Task<IActionResult> Search(
        string culture,
        [FromBody] AiSearchRequest request,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        if (string.IsNullOrWhiteSpace(request.Description))
            return BadRequest(new { error = "Description is required." });

        if (request.Description.Length > 500)
            return BadRequest(new { error = "Description is too long (max 500 characters)." });

        var result = await _aiSearchService.SearchAsync(request.Description, culture, cancellationToken);

        if (!result.Success)
            return StatusCode(503, new { error = result.ErrorMessage });

        return Ok(new { redirectUrl = result.RedirectUrl });
    }
}

public sealed record AiSearchRequest(string Description);
