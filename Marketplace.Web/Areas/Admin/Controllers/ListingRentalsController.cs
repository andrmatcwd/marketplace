using Marketplace.Modules.Listings.Application.Rentals.Commands;
using Marketplace.Modules.Listings.Application.Rentals.Queries;
using Marketplace.Web.Areas.Admin.Models.Rentals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ListingRentalsController : Controller
{
    private readonly ISender _sender;

    public ListingRentalsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Edit(int listingId, CancellationToken cancellationToken)
    {
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
        if (!ModelState.IsValid)
            return View(model);

        var features = model.FeaturesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new SaveRentalCommand(
            model.ListingId,
            model.Price,
            model.Rooms,
            model.Area,
            model.Floor,
            features), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    public async Task<IActionResult> DeleteRental(int listingId, CancellationToken cancellationToken)
    {
        var rental = await _sender.Send(new GetRentalByListingIdQuery(listingId), cancellationToken);
        if (rental is null)
            return NotFound();

        ViewBag.ListingId = listingId;
        return View(rental);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteRentalConfirmed")]
    public async Task<IActionResult> DeleteRentalConfirmed(int listingId, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteRentalCommand(listingId), cancellationToken);
        return RedirectToAction(nameof(Edit), new { listingId });
    }

    public IActionResult CreateRoom(int rentalId, int listingId)
    {
        var model = new RentalRoomFormVm
        {
            RentalId = rentalId,
            ListingId = listingId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoom(RentalRoomFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var amenities = model.AmenitiesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new CreateRentalRoomCommand(
            model.RentalId,
            model.Title,
            model.Description,
            model.Price,
            model.Area,
            model.Guests,
            model.Beds,
            amenities,
            []), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    public async Task<IActionResult> EditRoom(int id, int listingId, CancellationToken cancellationToken)
    {
        var room = await _sender.Send(new GetRentalRoomByIdQuery(id), cancellationToken);
        if (room is null)
            return NotFound();

        var model = new RentalRoomFormVm
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
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoom(int id, RentalRoomFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        var amenities = model.AmenitiesText?
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList() ?? [];

        await _sender.Send(new EditRentalRoomCommand(
            id,
            model.Title,
            model.Description,
            model.Price,
            model.Area,
            model.Guests,
            model.Beds,
            amenities,
            []), cancellationToken);

        return RedirectToAction(nameof(Edit), new { listingId = model.ListingId });
    }

    public async Task<IActionResult> DeleteRoom(int id, int listingId, CancellationToken cancellationToken)
    {
        var room = await _sender.Send(new GetRentalRoomByIdQuery(id), cancellationToken);
        if (room is null)
            return NotFound();

        ViewBag.ListingId = listingId;
        return View(room);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteRoomConfirmed")]
    public async Task<IActionResult> DeleteRoomConfirmed(int id, int listingId, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteRentalRoomCommand(id), cancellationToken);
        return RedirectToAction(nameof(Edit), new { listingId });
    }
}
