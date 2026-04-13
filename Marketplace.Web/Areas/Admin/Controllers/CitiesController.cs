using Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.DeleteCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCityById;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Areas.Admin.Models.Cities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class CitiesController : Controller
{
    private readonly ISender _sender;
    private readonly ListingsDbContext _db;

    public CitiesController(ISender sender, ListingsDbContext db)
    {
        _sender = sender;
        _db = db;
    }

    public async Task<IActionResult> Index(string? search, int? regionId, CancellationToken cancellationToken)
    {
        var cities = await _sender.Send(new GetCitiesByFilterQuery(new CityFilter
        {
            Search = search,
            Page = 1,
            PageSize = int.MaxValue
        }), cancellationToken);


        ViewBag.Regions = await GetRegionsSelectList(cancellationToken);

        var model = new CityIndexVm
        {
            Search = search,
            RegionId = regionId,
            Items = cities.Items
                .Select(x => new CityListItemVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    //RegionId = x.RegionSlug
                }).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        ViewBag.Regions = await GetRegionsSelectList(cancellationToken);
        return View(new CityFormVm());
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CityFormVm model, CancellationToken cancellationToken)
    {
        ViewBag.Regions = await GetRegionsSelectList(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var entity = new City
        {
            RegionId = model.RegionId,
            Name = model.Name.Trim(),
            Slug = model.Slug.Trim().ToLowerInvariant()
        };

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new CreateCityCommand(
            model.RegionId,
            model.Name
        ), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var city = await _sender.Send(new GetCityByIdQuery(id), cancellationToken);

        if (city is null) return NotFound();

        ViewBag.Regions = await GetRegionsSelectList(cancellationToken);

        return View(new CityFormVm
        {
            Id = city.Id,
            //RegionId = city.RegionId,
            Name = city.Name,
            Slug = city.Slug
        });
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CityFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        ViewBag.Regions = await GetRegionsSelectList(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new EditCityCommand(
            id,
            model.RegionId,
            model.Name
        ), cancellationToken);

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
            //RegionId = city.RegionId
        });
    }

    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteCityCommand(id), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetRegionsSelectList(CancellationToken cancellationToken)
    {
        return await _db.Regions
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);
    }
}