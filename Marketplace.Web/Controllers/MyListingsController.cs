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
using Marketplace.Web.Models.MyListings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Marketplace.Web.Controllers;

[Authorize]
public class MyListingsController : Controller
{
    private readonly ISender _sender;

    public MyListingsController(ISender sender) => _sender = sender;

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetListingsByFilterQuery(new ListingFilter
        {
            SellerId = CurrentUserId,
            PageSize = 100
        }), cancellationToken);

        return View(new MyListingIndexVm
        {
            TotalCount = result.TotalCount,
            Items = result.Items.Select(x => new MyListingListItemVm
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                Status = x.Status,
                CategoryName = x.CategoryName,
                CityName = x.CityName,
                PrimaryImageUrl = x.ImageUrls.FirstOrDefault(),
                CreatedUtc = x.CreatedUtc
            }).ToList()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        return View(new MyListingFormVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MyListingFormVm model, CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        var id = await _sender.Send(new CreateListingCommand(
            model.Title,
            Slug: null,
            model.ShortDescription,
            model.Description,
            model.Phone,
            model.Email,
            model.Website,
            model.Address,
            model.Latitude,
            model.Longitude,
            SellerId: CurrentUserId,
            SubscriptionType: SubscriptionType.Free,
            Status: ListingStatus.Active,
            model.CategoryId,
            model.SubCategoryId,
            model.CityId), cancellationToken);

        TempData["Success"] = "Listing created.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null || listing.SellerId != CurrentUserId) return NotFound();

        await FillLookups(cancellationToken);

        return View(new MyListingFormVm
        {
            Id = listing.Id,
            Title = listing.Title,
            ShortDescription = listing.ShortDescription,
            Description = listing.Description,
            Address = listing.AddressLine,
            Phone = listing.Phone,
            Email = listing.Email,
            Website = listing.Website,
            CategoryId = listing.CategoryId,
            SubCategoryId = listing.SubCategoryId,
            CityId = listing.CityId,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MyListingFormVm model, CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null || listing.SellerId != CurrentUserId) return NotFound();

        await FillLookups(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditListingCommand(
            id,
            model.Title,
            Slug: null,
            model.ShortDescription,
            model.Description,
            model.Phone,
            model.Email,
            model.Website,
            model.Address,
            model.Latitude,
            model.Longitude,
            SellerId: CurrentUserId,
            listing.SubscriptionType,
            listing.Status,
            model.CategoryId,
            model.SubCategoryId,
            model.CityId), cancellationToken);

        TempData["Success"] = "Changes saved.";
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(id), cancellationToken);
        if (listing is null || listing.SellerId != CurrentUserId) return NotFound();

        await _sender.Send(new DeleteListingCommand(id), cancellationToken);
        TempData["Success"] = "Listing deleted.";
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
    }
}
