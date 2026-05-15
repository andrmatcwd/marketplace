using Marketplace.Web.Authorization;
using Marketplace.Modules.Listings.Application.Reviews.Commands.DeleteReview;
using Marketplace.Modules.Listings.Application.Reviews.Filters;
using Marketplace.Modules.Listings.Application.Reviews.Queries.GetById;
using Marketplace.Modules.Listings.Application.Reviews.Queries.GetReviewsByFilter;
using Marketplace.Web.Areas.Admin.Models.Reviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPolicies.ModeratorOrAbove)]
public sealed class ReviewsController : Controller
{
    private readonly ISender _sender;

    public ReviewsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(int? listingId, int? rating, int page = 1, CancellationToken cancellationToken = default)
    {
        var filter = new ReviewFilter
        {
            ListingId = listingId,
            Rating = rating,
            Page = page,
            PageSize = 25
        };

        var result = await _sender.Send(new GetReviewsByFilterQuery(filter), cancellationToken);

        var items = result.Items.Select(r => new ReviewListItemVm
        {
            Id = r.Id,
            ListingId = r.ListingId,
            ListingTitle = r.ListingTitle,
            AuthorName = r.AuthorName,
            Text = r.Text,
            Rating = r.Rating,
            CreatedAtUtc = r.CreatedAtUtc
        }).ToList();

        ViewBag.Page = result.Page;
        ViewBag.TotalPages = result.TotalPages;
        ViewBag.TotalCount = result.TotalCount;
        ViewBag.ListingId = listingId;
        ViewBag.Rating = rating;

        return View(items);
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var review = await _sender.Send(new GetReviewByIdQuery(id), cancellationToken);
        if (review is null || review.Id == 0)
            return NotFound();

        return View(new ReviewListItemVm
        {
            Id = review.Id,
            ListingId = review.ListingId,
            ListingTitle = review.ListingTitle,
            AuthorName = review.AuthorName,
            Text = review.Text,
            Rating = review.Rating,
            CreatedAtUtc = review.CreatedAtUtc
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteReviewCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
