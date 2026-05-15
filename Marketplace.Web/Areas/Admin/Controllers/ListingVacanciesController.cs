using Marketplace.Web.Authorization;
using Marketplace.Modules.Listings.Application.Vacancies.Commands;
using Marketplace.Modules.Listings.Application.Vacancies.Queries;
using Marketplace.Web.Areas.Admin.Models.Vacancies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPolicies.ManagerOrAbove)]
public class ListingVacanciesController : Controller
{
    private readonly ISender _sender;

    public ListingVacanciesController(ISender sender)
    {
        _sender = sender;
    }

    public async Task<IActionResult> Index(int listingId, CancellationToken cancellationToken)
    {
        var vacancies = await _sender.Send(new GetVacanciesByListingIdQuery(listingId), cancellationToken);

        var items = vacancies.Select(v => new VacancyListItemVm
        {
            Id = v.Id,
            ListingId = v.ListingId,
            Title = v.Title,
            EmploymentType = v.EmploymentType,
            SalaryText = v.SalaryText,
            LocationText = v.LocationText
        }).ToList();

        ViewBag.ListingId = listingId;
        return View(items);
    }

    public IActionResult Create(int listingId)
    {
        return View(new VacancyFormVm { ListingId = listingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VacancyFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _sender.Send(new CreateVacancyCommand(
            model.ListingId,
            model.Title,
            model.Description,
            model.EmploymentType,
            model.SalaryText,
            model.LocationText), cancellationToken);

        return RedirectToAction(nameof(Index), new { listingId = model.ListingId });
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var vacancy = await _sender.Send(new GetVacancyByIdQuery(id), cancellationToken);
        if (vacancy is null)
            return NotFound();

        var model = new VacancyFormVm
        {
            Id = vacancy.Id,
            ListingId = vacancy.ListingId,
            Title = vacancy.Title,
            Description = vacancy.Description,
            EmploymentType = vacancy.EmploymentType,
            SalaryText = vacancy.SalaryText,
            LocationText = vacancy.LocationText
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VacancyFormVm model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _sender.Send(new EditVacancyCommand(
            id,
            model.Title,
            model.Description,
            model.EmploymentType,
            model.SalaryText,
            model.LocationText), cancellationToken);

        return RedirectToAction(nameof(Index), new { listingId = model.ListingId });
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var vacancy = await _sender.Send(new GetVacancyByIdQuery(id), cancellationToken);
        if (vacancy is null)
            return NotFound();

        var model = new VacancyFormVm
        {
            Id = vacancy.Id,
            ListingId = vacancy.ListingId,
            Title = vacancy.Title,
            Description = vacancy.Description,
            EmploymentType = vacancy.EmploymentType,
            SalaryText = vacancy.SalaryText,
            LocationText = vacancy.LocationText
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id, int listingId, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteVacancyCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index), new { listingId });
    }
}
