using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Models.Shared;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.ViewComponents;

public sealed class NavCitiesViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public NavCitiesViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var culture = CultureHelper.Current();
        var cities = await _mediator.Send(new GetCatalogCitiesQuery(), CancellationToken.None);

        var vms = cities
            .Select(x => new NavCityOptionVm
            {
                Name = x.Name,
                Slug = x.Slug,
                Url = $"/{culture}/{x.Slug}"
            })
            .ToList();

        return View(vms);
    }
}
