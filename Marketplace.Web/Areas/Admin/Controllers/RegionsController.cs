using Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;
using Marketplace.Modules.Listings.Application.Regions.Commands.DeleteRegion;
using Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;
using Marketplace.Modules.Listings.Application.Regions.Filters;
using Marketplace.Modules.Listings.Application.Regions.Queries.GetById;
using Marketplace.Modules.Listings.Application.Regions.Queries.GetRegionsByFilter;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Areas.Admin.Models.Regions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class RegionsController : Controller
{
    private readonly ISender _sender;

    public RegionsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        var regions = await _sender.Send(new GetRegionsByFilterQuery(new RegionFilter
        {
            Search = search,
            Page = 1,
            PageSize = 3
        }), cancellationToken);

        var model = new RegionIndexVm
        {
            Search = search,
            Items = regions.Items
                .Select(x => new RegionListItemVm
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                }).ToList()
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new RegionFormVm());
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegionFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new CreateRegionCommand(
            model.Name),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var entity = await _sender.Send(new GetRegionByIdQuery(id), cancellationToken);
        if (entity is null) return NotFound();

        return View(new RegionFormVm
        {
            Id = entity.Id,
            Name = entity.Name,
            Slug = entity.Slug
        });
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, RegionFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new EditRegionCommand(
            id,
            model.Name),
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var region = await _sender.Send(new GetRegionByIdQuery(id), cancellationToken);

        if (region is null) return NotFound();

        return View(new RegionListItemVm
        {
            Id = region.Id,
            Name = region.Name,
            Slug = region.Slug,
        });
    }

    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteRegionCommand(id), cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}