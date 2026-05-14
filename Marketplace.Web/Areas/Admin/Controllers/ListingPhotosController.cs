using Marketplace.Modules.Listings.Application.Listings.Commands.ManageImages;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingImages;
using Marketplace.Web.Services.Media;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ListingPhotosController : Controller
{
    private readonly ISender _sender;
    private readonly IPhotoStorageService _storage;

    public ListingPhotosController(ISender sender, IPhotoStorageService storage)
    {
        _sender = sender;
        _storage = storage;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int listingId, CancellationToken cancellationToken)
    {
        var images = await _sender.Send(new GetListingImagesQuery(listingId), cancellationToken);
        ViewBag.ListingId = listingId;
        return View(images);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Upload(int listingId, IFormFile file, CancellationToken cancellationToken)
    {
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
    public async Task<IActionResult> Delete(int photoId, CancellationToken cancellationToken)
    {
        var url = await _sender.Send(new DeleteListingImageCommand(photoId), cancellationToken);
        if (url is not null) _storage.Delete(url);
        return Ok(new { success = true });
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> SetPrimary(int listingId, int photoId, CancellationToken cancellationToken)
    {
        await _sender.Send(new SetPrimaryImageCommand(listingId, photoId), cancellationToken);
        return Ok(new { success = true });
    }
}
