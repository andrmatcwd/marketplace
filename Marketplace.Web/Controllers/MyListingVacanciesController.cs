using Marketplace.Modules.Listings.Application.Listings.Queries.GetById;
using Marketplace.Modules.Listings.Application.Vacancies.Commands;
using Marketplace.Modules.Listings.Application.Vacancies.Queries;
using Marketplace.Web.Areas.Admin.Models.Vacancies;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Web.Controllers;

[Authorize]
public class MyListingVacanciesController : Controller
{
    private readonly ISender _sender;

    public MyListingVacanciesController(ISender sender) => _sender = sender;

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

    [HttpGet]
    public async Task<IActionResult> Create(int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();
        return View(new VacancyFormVm { ListingId = listingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VacancyFormVm model, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(model.ListingId, cancellationToken)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new CreateVacancyCommand(
            model.ListingId, model.Title, model.Description,
            model.EmploymentType, model.SalaryText, model.LocationText), cancellationToken);

        return RedirectToAction(nameof(Index), new { listingId = model.ListingId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var vacancy = await _sender.Send(new GetVacancyByIdQuery(id), cancellationToken);
        if (vacancy is null) return NotFound();
        if (!await OwnsListingAsync(vacancy.ListingId, cancellationToken)) return NotFound();

        return View(new VacancyFormVm
        {
            Id = vacancy.Id,
            ListingId = vacancy.ListingId,
            Title = vacancy.Title,
            Description = vacancy.Description,
            EmploymentType = vacancy.EmploymentType,
            SalaryText = vacancy.SalaryText,
            LocationText = vacancy.LocationText
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, VacancyFormVm model, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(model.ListingId, cancellationToken)) return NotFound();
        if (!ModelState.IsValid) return View(model);

        await _sender.Send(new EditVacancyCommand(
            id, model.Title, model.Description,
            model.EmploymentType, model.SalaryText, model.LocationText), cancellationToken);

        return RedirectToAction(nameof(Index), new { listingId = model.ListingId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int listingId, CancellationToken cancellationToken)
    {
        if (!await OwnsListingAsync(listingId, cancellationToken)) return NotFound();
        await _sender.Send(new DeleteVacancyCommand(id), cancellationToken);
        return RedirectToAction(nameof(Index), new { listingId });
    }
}
