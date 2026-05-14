using Marketplace.Modules.Appointments.Application.Appointments.Commands.CreateAppointment;
using Marketplace.Web.Models.Appointment;
using Marketplace.Web.Seo;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class AppointmentController : Controller
{
    private readonly ISender _sender;

    public AppointmentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/appointment")]
    public IActionResult Index(string culture)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        var isUk = culture == "uk";

        ViewData["Culture"] = culture;
        ViewData["Seo"] = new PageSeoData
        {
            Title = isUk
                ? "Подати заявку на розміщення бізнесу"
                : "Подать заявку на размещение бизнеса",
            Description = isUk
                ? "Заповніть форму, щоб розмістити свій бізнес у каталозі Marketplace. Наша команда зв'яжеться з вами найближчим часом."
                : "Заполните форму, чтобы разместить свой бизнес в каталоге Marketplace. Наша команда свяжется с вами в ближайшее время.",
            Robots = "index, follow"
        };

        return View(new AppointmentFormVm());
    }

    [HttpPost("/{culture:regex(^uk|ru$)}/appointment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string culture, AppointmentFormVm model,
        CancellationToken cancellationToken = default)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        ViewData["Culture"] = culture;

        if (!ModelState.IsValid)
        {
            ViewData["Seo"] = new PageSeoData { Robots = "noindex, nofollow" };
            return View(model);
        }

        await _sender.Send(new CreateAppointmentCommand(
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

        return RedirectToAction(nameof(Success), new { culture });
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/appointment/success")]
    public IActionResult Success(string culture)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        ViewData["Culture"] = culture;
        ViewData["Seo"] = new PageSeoData
        {
            Title  = culture == "uk" ? "Заявку отримано" : "Заявка получена",
            Robots = "noindex, nofollow"
        };
        return View();
    }
}
