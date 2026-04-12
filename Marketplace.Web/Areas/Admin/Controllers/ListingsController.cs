using Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Marketplace.Modules.Listings.Infrastructure.Persistence;
using Marketplace.Web.Areas.Admin.Models.Listings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize]
public class ListingsController : Controller
{
    private readonly ListingsDbContext _db;
    private readonly ISender _sender;

    public ListingsController(ListingsDbContext db, ISender sender)
    {
        _db = db;
        _sender = sender;
    }

    public async Task<IActionResult> Index(
        string? search,
        int? categoryId,
        int? subCategoryId,
        int? locationId,
        ListingStatus? status,
        CancellationToken cancellationToken)
    {
        var listings = await _sender.Send(new GetListingsByFilterQuery(new ListingFilter
        {
            Search = search,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            LocationId = locationId,
            IsActive = true,
            Page = 1,
            PageSize = int.MaxValue
        }), cancellationToken);

        await FillLookups(cancellationToken);

        var model = new ListingIndexVm
        {
            Search = search,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            LocationId = locationId,
            Status = status,
            Items = listings.Items
                .Select(x => new ListingListItemVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    // CategoryName = x.Category.Name,
                    // SubCategoryName = x.SubCategory.Name,
                    // CityName = x.City.Name,
                    // Price = x.Price,
                    // Currency = x.Currency,
                    // Status = x.Status,
                    // CreatedUtc = x.CreatedAtUtc
                }).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        return View(new ListingFormVm());
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ListingFormVm model, CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _sender.Send(new CreateListingCommand(
            model.Title,
            model.Description,
            model.Price,
            model.SellerId,
            model.CategoryId,
            model.SubCategoryId
        ), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null) return NotFound();

        await FillLookups(cancellationToken);

        return View(new ListingFormVm
        {
            Id = listing.Id,
            Title = listing.Title,
            Price = listing.Price,
        });
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ListingFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        await FillLookups(cancellationToken);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null) return NotFound();

        await _sender.Send(new EditListingCommand(
            id,
            model.Title,
            model.Description,
            model.Price,
            model.SellerId,
            true
        ), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null) return NotFound();

        return View(new ListingListItemVm
        {
            Id = listing.Id,
            Title = listing.Title,
        });
    }

    [HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteListingCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLookups(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);

        ViewBag.SubCategories = await _db.SubCategories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);

        ViewBag.Cities = await _db.Cities
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            })
            .ToListAsync(cancellationToken);

        ViewBag.Statuses = Enum.GetValues<ListingStatus>()
            .Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            })
            .ToList();

        ViewBag.SubscriptionTypes = Enum.GetValues<SubscriptionType>()
            .Select(x => new SelectListItem
            {
                Value = x.ToString(),
                Text = x.ToString()
            })
            .ToList();
    }
}