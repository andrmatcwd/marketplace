using Marketplace.Modules.Listings.Application.Listings.Commands.ManageImages;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingImages;
using Marketplace.Web.Services.Media;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Web.Controllers;

[Authorize]
public class MyListingPhotosController : Controller
{
    private readonly ISender _sender;
    private readonly IPhotoStorageService _storage;

    public MyListingPhotosController(ISender sender, IPhotoStorageService storage)
    {
        _sender = sender;
        _storage = storage;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    private async Task<bool> OwnsListingAsync(int listingId, CancellationToken ct)
    {
        var listing = await _sender.Send(new GetListingByIdQuery(listingId), ct);
        return listing?.SellerId == CurrentUserId;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();

        var images = await _sender.Send(new GetListingImagesQuery(listingId), cancellationToken);
        ViewBag.ListingId = listingId;
        return View(images);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Upload(int listingId, IFormFile file, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return Forbid();

        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        try
        {
            var url = await _storage.SaveAsync(file, "listings", cancellationToken);
            var imageId = await _sender.Send(new AddListingImageCommand(listingId, url, null), cancellationToken);
            return Ok(new { id = imageId, url });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Delete(int listingId, int photoId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return Forbid();

        var url = await _sender.Send(new DeleteListingImageCommand(photoId), cancellationToken);
        if (url is not null) _storage.Delete(url);
        return Ok(new { success = true });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetPrimary(int listingId, int photoId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return Forbid();

        await _sender.Send(new SetPrimaryImageCommand(listingId, photoId), cancellationToken);
        return Ok(new { success = true });
    }
}
