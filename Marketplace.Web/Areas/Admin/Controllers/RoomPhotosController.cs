using Marketplace.Web.Authorization;
using Marketplace.Modules.Listings.Application.Rentals.Commands;
using Marketplace.Modules.Listings.Application.Rentals.Queries;
using Marketplace.Web.Services.Media;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPolicies.ManagerOrAbove)]
public class RoomPhotosController : Controller
{
    private readonly ISender _sender;
    private readonly IPhotoStorageService _storage;

    public RoomPhotosController(ISender sender, IPhotoStorageService storage)
    {
        _sender = sender;
        _storage = storage;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int roomId, int listingId, CancellationToken cancellationToken)
    {
        var room = await _sender.Send(new GetRentalRoomByIdQuery(roomId), cancellationToken);
        if (room is null) return NotFound();

        ViewBag.RoomId = roomId;
        ViewBag.ListingId = listingId;
        ViewBag.RoomTitle = room.Title;
        return View(room.ImageUrls.ToList());
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Upload(int roomId, IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        try
        {
            var url = await _storage.SaveAsync(file, "rooms", cancellationToken);
            await _sender.Send(new AddRoomImageCommand(roomId, url), cancellationToken);
            return Ok(new { url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(int roomId, [FromBody] DeleteRoomPhotoRequest request, CancellationToken cancellationToken)
    {
        var url = await _sender.Send(new DeleteRoomImageCommand(roomId, request.Url), cancellationToken);
        if (url is not null) _storage.Delete(url);
        return Ok(new { success = true });
    }
}

public sealed record DeleteRoomPhotoRequest(string Url);
