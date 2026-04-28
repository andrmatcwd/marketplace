using Marketplace.Web.Data;
using Marketplace.Web.Options;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/location")]
public sealed class LocationApiController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly LocationDefaultsOptions _defaults;

    public LocationApiController(
        ApplicationDbContext dbContext,
        IOptions<LocationDefaultsOptions> defaults)
    {
        _dbContext = dbContext;
        _defaults = defaults.Value;
    }

    [HttpGet("bootstrap")]
    public async Task<IActionResult> Bootstrap(string culture, CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        var defaultCity = await _dbContext.Cities
            .AsNoTracking()
            .Where(x => x.IsPublished && x.Slug == _defaults.DefaultCitySlug)
            .Select(x => new
            {
                city = x.Slug,
                cityName = x.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (defaultCity is null)
        {
            defaultCity = await _dbContext.Cities
                .AsNoTracking()
                .Where(x => x.IsPublished)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    city = x.Slug,
                    cityName = x.Name
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        return Ok(defaultCity ?? new { city = "kyiv", cityName = "Kyiv" });
    }
}