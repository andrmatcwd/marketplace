using Marketplace.Web.Authorization;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;
using Marketplace.Modules.Listings.Application.Subscriptions.Commands.AssignSubscription;
using Marketplace.Modules.Listings.Application.Subscriptions.Commands.CancelSubscription;
using Marketplace.Modules.Listings.Application.Subscriptions.Filters;
using Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlans;
using Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionsByFilter;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Marketplace.Web.Areas.Admin.Models.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPolicies.ManagerOrAbove)]
public class SubscriptionsController : Controller
{
    private readonly ISender _sender;

    public SubscriptionsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(
        int? listingId,
        int? planId,
        SubscriptionStatus? status,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetSubscriptionsByFilterQuery(new SubscriptionFilter
        {
            ListingId = listingId,
            PlanId = planId,
            Status = status,
            PageSize = 50
        }), cancellationToken);

        await FillLookups(cancellationToken);

        return View(new SubscriptionIndexVm
        {
            ListingId = listingId,
            PlanId = planId,
            Status = status,
            TotalCount = result.TotalCount,
            Items = result.Items.Select(x => new SubscriptionListItemVm
            {
                Id = x.Id,
                ListingId = x.ListingId,
                ListingTitle = x.ListingTitle,
                PlanName = x.PlanName,
                SubscriptionType = x.SubscriptionType,
                StartsAt = x.StartsAt,
                ExpiresAt = x.ExpiresAt,
                Status = x.Status,
                Notes = x.Notes,
                CreatedAtUtc = x.CreatedAtUtc
            }).ToList()
        });
    }

    public async Task<IActionResult> Assign(int? listingId, CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        return View(new AssignSubscriptionVm
        {
            ListingId = listingId ?? 0,
            StartsAt = DateTime.UtcNow.Date
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(AssignSubscriptionVm model, CancellationToken cancellationToken)
    {
        await FillLookups(cancellationToken);
        if (!ModelState.IsValid) return View(model);

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        await _sender.Send(new AssignSubscriptionCommand(
            model.ListingId,
            model.PlanId,
            DateTime.SpecifyKind(model.StartsAt, DateTimeKind.Utc),
            userId,
            model.Notes), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new CancelSubscriptionCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private async Task FillLookups(CancellationToken cancellationToken)
    {
        var plansTask = _sender.Send(new GetSubscriptionPlansQuery(), cancellationToken);
        var listingsTask = _sender.Send(new GetListingsByFilterQuery(new ListingFilter { PageSize = 500 }), cancellationToken);

        await Task.WhenAll(plansTask, listingsTask);

        ViewBag.Plans = plansTask.Result
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = $"{x.Name} ({x.SubscriptionType}, {x.DurationDays}d, {x.PriceUah}₴)"
            }).ToList();

        ViewBag.Listings = listingsTask.Result.Items
            .OrderBy(x => x.Title)
            .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = $"#{x.Id} {x.Title}" })
            .ToList();

        ViewBag.Statuses = Enum.GetValues<SubscriptionStatus>()
            .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() })
            .ToList();
    }
}
