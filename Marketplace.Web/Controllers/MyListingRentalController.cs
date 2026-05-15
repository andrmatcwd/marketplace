using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Rentals.Commands;
using Marketplace.Modules.Listings.Application.Rentals.Queries;
using Marketplace.Web.Areas.Admin.Models.Rentals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Web.Controllers;

[Authorize]
public class MyListingRentalController : Controller
{
    private readonly ISender _sender;

    public MyListingRentalController(ISender sender) => _sender = sender;

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private async Task<bool> OwnsListingAsync(int listingId, CancellationToken ct)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(listingId), ct);
        return listing?.SellerId == CurrentUserId;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();

        var rental = await _sender.Send(new GetRentalByListingIdQuery(listingId), cancellationToken);

        var model = new RentalFormVm
        {
            ListingId = listingId,
            Price = rental?.Price,
            Rooms = rental?.Rooms,
            Area = rental?.Area,
            Floor = rental?.Floor,
            FeaturesText = rental is not null ? string.Join('\n', rental.Features) : null
        };

        ViewBag.RoomOptions = rental?.RoomOptions ?? [];
        ViewBag.RentalId = rental?.Id;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RentalFormVm model, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(model.ListingId, cancellationToken)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var features = model.FeaturesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new SaveRentalCommand(
            model.ListingId, model.Price, model.Rooms, model.Area, model.Floor, features), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRental(int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();
        await _sender.Send(new DeleteRentalCommand(listingId), cancellationToken);
        return RedirectToAction(nameof(Edit), new { listingId });
    }

    [HttpGet]
    public async Task<IActionResult> CreateRoom(int rentalId, int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();
        return View(new RentalRoomFormVm { RentalId = rentalId, ListingId = listingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(RentalRoomFormVm model, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(model.ListingId, cancellationToken)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var amenities = model.AmenitiesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new CreateRentalRoomCommand(
            model.RentalId, model.Title, model.Description, model.Price,
            model.Area, model.Guests, model.Beds, amenities, []), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    [HttpGet]
    public async Task<IActionResult> EditRoom(int id, int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();

        var room = await _sender.Send(new GetRentalRoomByIdQuery(id), cancellationToken);
        if (room is null) return NotFound();

        return View(new RentalRoomFormVm
        {
            Id = room.Id,
            RentalId = room.RentalId,
            ListingId = listingId,
            Title = room.Title,
            Description = room.Description,
            Price = room.Price,
            Area = room.Area,
            Guests = room.Guests,
            Beds = room.Beds,
            AmenitiesText = string.Join('\n', room.Amenities)
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoom(int id, RentalRoomFormVm model, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(model.ListingId, cancellationToken)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var amenities = model.AmenitiesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new EditRentalRoomCommand(
            id, model.Title, model.Description, model.Price,
            model.Area, model.Guests, model.Beds, amenities, []), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteRoom(int id, int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();
        await _sender.Send(new DeleteRentalRoomCommand(id), cancellationToken);
        return RedirectToAction(nameof(Edit), new { listingId });
    }
}
