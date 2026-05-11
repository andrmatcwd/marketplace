using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;
using Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;
using Marketplace.Modules.Listings.Domain.Enums.Listing;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Marketplace.Web.Areas.Admin.Models.Listings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ListingsController : Controller
{
    private readonly ISender _sender;

    public ListingsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(
        string? search,
        int? categoryId,
        int? subCategoryId,
        int? cityId,
        ListingStatus? status,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetListingsByFilterQuery(new ListingFilter
        {
            Search = search,
            PageSize = 25
        }), cancellationToken);

        await FillLookups(cancellationToken);

        return View(new ListingIndexVm
        {
            Search = search,
            CategoryId = categoryId,
            SubCategoryId = subCategoryId,
            CityId = cityId,
            Status = status,
            Items = result.Items
                .Select(x => new ListingListItemVm
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    CategoryName = x.CategoryName,
                    SubCategoryName = x.SubCategoryName,
                    CityName = x.CityName,
                    Status = x.Status,
                    CreatedUtc = x.CreatedUtc
                })
                .ToList()
        });
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        return View(new ListingFormVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ListingFormVm model, CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateListingCommand(
            model.Title,
            model.Slug,
            model.ShortDescription,
            model.Description,
            model.Phone,
            model.Email,
            model.Website,
            model.Address,
            model.Latitude,
            model.Longitude,
            model.SellerId,
            model.SubscriptionType,
            model.Status,
            model.CategoryId,
            model.SubCategoryId,
            model.CityId), cancellationToken);

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
            Slug = listing.Slug,
            Description = listing.Description,
            Status = listing.Status,
            SubscriptionType = listing.SubscriptionType,
            SellerId = listing.SellerId,
            Address = listing.AddressLine,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ListingFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        await FillLookups(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditListingCommand(
            id,
            model.Title,
            model.Slug,
            model.ShortDescription,
            model.Description,
            model.Phone,
            model.Email,
            model.Website,
            model.Address,
            model.Latitude,
            model.Longitude,
            model.SellerId,
            model.SubscriptionType,
            model.Status,
            model.CategoryId,
            model.SubCategoryId,
            model.CityId), cancellationToken);

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
            Slug = listing.Slug,
            CategoryName = listing.CategoryName,
            SubCategoryName = listing.SubCategoryName,
            CityName = listing.CityName,
            Status = listing.Status,
            CreatedUtc = listing.CreatedUtc
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteListingCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLookups(CancellationToken cancellationToken)
    {
        var categoriesTask = _sender.Send(new GetCategoriesByFilterQuery(new CategoryFilter { PageSize = 100 }), cancellationToken);
        var subCategoriesTask = _sender.Send(new GetSubCategoriesByFilterQuery(new SubCategoryFilter { PageSize = 100 }), cancellationToken);
        var citiesTask = _sender.Send(new GetCitiesByFilterQuery(new CityFilter { PageSize = 100 }), cancellationToken);

        await Task.WhenAll(categoriesTask, subCategoriesTask, citiesTask);

        ViewBag.Categories = categoriesTask.Result.Items
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            .ToList();

        ViewBag.SubCategories = subCategoriesTask.Result.Items
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"{x.CategoryName} / {x.Name}" })
            .ToList();

        ViewBag.Cities = citiesTask.Result.Items
            .OrderBy(x => x.Name)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name })
            .ToList();

        ViewBag.Statuses = Enum.GetValues<ListingStatus>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();

        ViewBag.SubscriptionTypes = Enum.GetValues<SubscriptionType>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();
    }
}
