using Marketplace.Modules.Appointments.Application.Appointments.Commands.CreateAppointment;
using Marketplace.Modules.Appointments.Application.Appointments.Commands.DeleteAppointment;
using Marketplace.Modules.Appointments.Application.Appointments.Commands.UpdateAppointment;
using Marketplace.Modules.Appointments.Application.Appointments.Filters;
using Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentById;
using Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentsByFilter;
using Marketplace.Modules.Appointments.Domain.Enums;
using Marketplace.Web.Areas.Admin.Models.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public sealed class AppointmentsController : Controller
{
    private readonly ISender _sender;
    private const int PageSize = 20;

    public AppointmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? search, int? status, int page = 1,
        CancellationToken cancellationToken = default)
    {
        var statusFilter = status.HasValue
            ? (AppointmentStatus?)status.Value
            : null;

        var result = await _sender.Send(new GetAppointmentsByFilterQuery(new AppointmentFilter
        {
            Search   = search,
            Status   = statusFilter,
            Page     = page,
            PageSize = PageSize
        }), cancellationToken);

        var vm = new AppointmentIndexVm
        {
            Search     = search,
            Status     = statusFilter,
            Page       = page,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            Items      = result.Items.Select(a => new AppointmentListItemVm
            {
                Id           = a.Id,
                BusinessName = a.BusinessName,
                ContactName  = a.ContactName,
                Phone        = a.Phone,
                Email        = a.Email,
                CategoryName = a.CategoryName,
                CityName     = a.CityName,
                Status       = a.Status,
                CreatedAtUtc = a.CreatedAtUtc
            }).ToList()
        };

        return View(vm);
    }

    [HttpGet]
    public IActionResult Create() => View(new AppointmentAdminFormVm());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppointmentAdminFormVm model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return View(model);

        var id = await _sender.Send(new CreateAppointmentCommand(
            model.BusinessName,
            model.ContactName,
            model.Phone,
            model.Email,
            model.CategoryName,
            model.CityName,
            model.Address,
            model.Website,
            model.Description
        ), cancellationToken);

        if (model.Status != AppointmentStatus.New
            || model.AdminNotes is not null)
        {
            await _sender.Send(new UpdateAppointmentCommand(id, model.Status, model.AdminNotes),
                cancellationToken);
        }

        TempData["Success"] = $"Заявку #{id} створено.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
    {
        var dto = await _sender.Send(new GetAppointmentByIdQuery(id), cancellationToken);
        if (dto is null) return NotFound();
        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
    {
        var dto = await _sender.Send(new GetAppointmentByIdQuery(id), cancellationToken);
        if (dto is null) return NotFound();

        return View(new AppointmentEditVm
        {
            Id         = dto.Id,
            Status     = dto.Status,
            AdminNotes = dto.AdminNotes
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppointmentEditVm model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new UpdateAppointmentCommand(id, model.Status, model.AdminNotes),
            cancellationToken);

        TempData["Success"] = "Заявку оновлено.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var dto = await _sender.Send(new GetAppointmentByIdQuery(id), cancellationToken);
        if (dto is null) return NotFound();
        return View(dto);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken = default)
    {
        await _sender.Send(new DeleteAppointmentCommand(id), cancellationToken);
        TempData["Success"] = "Заявку видалено.";
        return RedirectToAction(nameof(Index));
    }
}
