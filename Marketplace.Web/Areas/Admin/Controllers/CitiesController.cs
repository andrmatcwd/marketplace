using Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.DeleteCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCityById;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Web.Areas.Admin.Models.Cities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CitiesController : Controller
{
    private readonly ISender _sender;

    public CitiesController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCitiesByFilterQuery(new CityFilter
        {
            Search = search,
            PageSize = 25
        }), cancellationToken);

        return View(new CityIndexVm
        {
            Search = search,
            Items = result.Items
                .Select(x => new CityListItemVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    ListingsCount = x.ListingsCount
                })
                .ToList()
        });
    }

    public IActionResult Create() => View(new CityFormVm());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateCityCommand(
            model.Name,
            model.Slug,
            model.Description,
            model.IsPublished,
            model.SortOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var city = await _sender.Send(new GetCityByIdQuery(id), cancellationToken);
        if (city is null) return NotFound();

        return View(new CityFormVm
        {
            Id = city.Id,
            Name = city.Name,
            Slug = city.Slug
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CityFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditCityCommand(
            id,
            model.Name,
            model.Slug,
            model.Description,
            model.IsPublished,
            model.SortOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var city = await _sender.Send(new GetCityByIdQuery(id), cancellationToken);
        if (city is null) return NotFound();

        return View(new CityListItemVm
        {
            Id = city.Id,
            Name = city.Name,
            Slug = city.Slug,
            ListingsCount = city.ListingsCount
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteCityCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
