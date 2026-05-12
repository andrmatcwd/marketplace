using Marketplace.Modules.Listings.Application.Subscriptions.Commands.CreateSubscriptionPlan;
using Marketplace.Modules.Listings.Application.Subscriptions.Commands.DeleteSubscriptionPlan;
using Marketplace.Modules.Listings.Application.Subscriptions.Commands.EditSubscriptionPlan;
using Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlanById;
using Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlans;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;
using Marketplace.Web.Areas.Admin.Models.SubscriptionPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SubscriptionPlansController : Controller
{
    private readonly ISender _sender;

    public SubscriptionPlansController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var plans = await _sender.Send(new GetSubscriptionPlansQuery(), cancellationToken);
        var items = plans.Select(x => new SubscriptionPlanListItemVm
        {
            Id = x.Id,
            Name = x.Name,
            SubscriptionType = x.SubscriptionType,
            PriceUah = x.PriceUah,
            DurationDays = x.DurationDays,
            IsActive = x.IsActive,
            DisplayOrder = x.DisplayOrder
        }).ToList();

        return View(items);
    }

    public IActionResult Create()
    {
        FillTierLookup();
        return View(new SubscriptionPlanFormVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SubscriptionPlanFormVm model, CancellationToken cancellationToken)
    {
        FillTierLookup();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateSubscriptionPlanCommand(
            model.Name,
            model.Description,
            (int)model.SubscriptionType,
            model.PriceUah,
            model.DurationDays,
            model.IsActive,
            model.DisplayOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var plan = await _sender.Send(new GetSubscriptionPlanByIdQuery(id), cancellationToken);
        if (plan is null) return NotFound();

        FillTierLookup();
        return View(new SubscriptionPlanFormVm
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            SubscriptionType = plan.SubscriptionType,
            PriceUah = plan.PriceUah,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive,
            DisplayOrder = plan.DisplayOrder
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SubscriptionPlanFormVm model, CancellationToken cancellationToken)
    {
        if (id != model.Id) return BadRequest();

        FillTierLookup();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditSubscriptionPlanCommand(
            id,
            model.Name,
            model.Description,
            (int)model.SubscriptionType,
            model.PriceUah,
            model.DurationDays,
            model.IsActive,
            model.DisplayOrder), cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var plan = await _sender.Send(new GetSubscriptionPlanByIdQuery(id), cancellationToken);
        if (plan is null) return NotFound();

        return View(new SubscriptionPlanListItemVm
        {
            Id = plan.Id,
            Name = plan.Name,
            SubscriptionType = plan.SubscriptionType,
            PriceUah = plan.PriceUah,
            DurationDays = plan.DurationDays,
            IsActive = plan.IsActive,
            DisplayOrder = plan.DisplayOrder
        });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteSubscriptionPlanCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    private void FillTierLookup()
    {
        ViewBag.Tiers = Enum.GetValues<SubscriptionType>()
            .Where(x => x != SubscriptionType.Free)
            .Select(x => new SelectListItem { Value = ((int)x).ToString(), Text = x.ToString() })
            .ToList();
    }
}
