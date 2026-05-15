using Marketplace.Web.Models.Vacancies;
using Marketplace.Web.Services.Vacancies;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class VacanciesController : Controller
{
    private readonly IVacanciesService _vacanciesService;

    public VacanciesController(IVacanciesService vacanciesService)
    {
        _vacanciesService = vacanciesService;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/vacancies")]
    [HttpGet("/{culture:regex(^uk|ru$)}/vacancies/page-{page:int}")]
    public async Task<IActionResult> Index(
        string culture,
        int? page,
        [FromQuery] VacanciesFilterVm filter,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        filter.Page = page ?? filter.Page;

        if (Request.Path.Value?.Contains("/page-1", StringComparison.OrdinalIgnoreCase) == true)
        {
            return RedirectPermanent($"/{culture}/vacancies");
        }

        var vm = await _vacanciesService.GetVacanciesPageAsync(culture, filter, cancellationToken);

        ViewData["Title"] = "Vacancies";

        return View(vm);
    }
}
