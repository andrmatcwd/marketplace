using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/location")]
public sealed class LocationApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly LocationDefaultsOptions _defaults;

    public LocationApiController(
        IMediator mediator,
        IOptions<LocationDefaultsOptions> defaults)
    {
        _mediator = mediator;
        _defaults = defaults.Value;
    }

    [HttpGet("bootstrap")]
    public async Task<IActionResult> Bootstrap(CancellationToken cancellationToken)
    {
        var defaultCity = await _mediator.Send(
            new GetCatalogCityBySlugQuery(_defaults.DefaultCitySlug), cancellationToken);

        if (defaultCity is not null)
            return Ok(new { city = defaultCity.Slug, cityName = defaultCity.Name });

        var cities = await _mediator.Send(new GetCatalogCitiesQuery(), cancellationToken);

        if (cities.Count > 0)
            return Ok(new { city = cities[0].Slug, cityName = cities[0].Name });

        return Ok(new { city = "kyiv", cityName = "Kyiv" });
    }
}
