using Marketplace.Web.Authorization;
using Marketplace.Modules.Notifications.Application.ContactRequests.Commands.DeleteContactRequest;
using Marketplace.Modules.Notifications.Application.ContactRequests.Commands.UpdateContactRequestStatus;
using Marketplace.Modules.Notifications.Application.ContactRequests.Filters;
using Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestById;
using Marketplace.Modules.Notifications.Application.ContactRequests.Queries.GetContactRequestsByFilter;
using Marketplace.Modules.Notifications.Domain.Enums;
using Marketplace.Web.Areas.Admin.Models.ContactRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPolicies.ManagerOrAbove)]
public class ContactRequestsController : Controller
{
    private readonly ISender _sender;

    public ContactRequestsController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(
        string? search,
        ContactRequestType? type,
        ContactRequestStatus? status,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetContactRequestsByFilterQuery(new ContactRequestFilter
        {
            Search = search,
            Type = type,
            Status = status,
            Page = page,
            PageSize = 25
        }), cancellationToken);

        return View(new ContactRequestIndexVm
        {
            Search = search,
            Type = type,
            Status = status,
            Page = page,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            Items = result.Items.Select(x => new ContactRequestListItemVm
            {
                Id = x.Id,
                Type = x.Type,
                Status = x.Status,
                CustomerName = x.CustomerName,
                CustomerPhone = x.CustomerPhone,
                CustomerCompany = x.CustomerCompany,
                ListingTitle = x.ListingTitle,
                CreatedAtUtc = x.CreatedAtUtc
            }).ToList()
        });
    }

    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
    {
        var dto = await _sender.Send(new GetContactRequestByIdQuery(id), cancellationToken);
        if (dto is null) return NotFound();

        return View(new ContactRequestDetailVm
        {
            Id = dto.Id,
            Type = dto.Type,
            Status = dto.Status,
            CustomerName = dto.CustomerName,
            CustomerPhone = dto.CustomerPhone,
            CustomerEmail = dto.CustomerEmail,
            CustomerCompany = dto.CustomerCompany,
            Message = dto.Message,
            ListingId = dto.ListingId,
            ListingTitle = dto.ListingTitle,
            AdminNotes = dto.AdminNotes,
            CreatedAtUtc = dto.CreatedAtUtc,
            ProcessedAtUtc = dto.ProcessedAtUtc
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(ContactRequestDetailVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return RedirectToAction(nameof(Detail), new { id = model.Id });

        await _sender.Send(new UpdateContactRequestStatusCommand(
            model.Id,
            model.Status,
            model.AdminNotes), cancellationToken);

        TempData["Success"] = "Status updated.";
        return RedirectToAction(nameof(Detail), new { id = model.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteContactRequestCommand(id), cancellationToken);
        TempData["Success"] = "Request deleted.";
        return RedirectToAction(nameof(Index));
    }
}
