using Marketplace.Modules.Notifications.Application.ContactRequests.Commands.CreateContactRequest;
using Marketplace.Modules.Notifications.Domain.Enums;
using Marketplace.Web.Models.Support;
using Marketplace.Web.Seo;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers;

public sealed class SupportController : Controller
{
    private readonly ISender _sender;

    public SupportController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/support")]
    public IActionResult Index(string culture)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        var isUk = culture == "uk";

        ViewData["Culture"] = culture;
        ViewData["Seo"] = new PageSeoData
        {
            Title = isUk ? "Служба підтримки" : "Служба поддержки",
            Description = isUk
                ? "Зв'яжіться зі службою підтримки Marketplace. Ми відповідаємо на запитання щодо каталогу, лістингів та технічних питань."
                : "Свяжитесь со службой поддержки Marketplace. Мы отвечаем на вопросы о каталоге, листингах и технических вопросах.",
            Robots = "index, follow"
        };

        return View(new SupportFormVm());
    }

    [HttpPost("/{culture:regex(^uk|ru$)}/support")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string culture, SupportFormVm model,
        CancellationToken cancellationToken = default)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        ViewData["Culture"] = culture;

        if (!ModelState.IsValid)
        {
            ViewData["Seo"] = new PageSeoData { Robots = "noindex, nofollow" };
            return View(model);
        }

        await _sender.Send(new CreateContactRequestCommand(
            Type: ContactRequestType.SupportRequest,
            CustomerName: model.Name,
            CustomerPhone: string.Empty,
            CustomerEmail: model.Email,
            CustomerCompany: model.Subject,
            Message: model.Message,
            ListingId: null,
            ListingTitle: null
        ), cancellationToken);

        return RedirectToAction(nameof(Success), new { culture });
    }

    [HttpGet("/{culture:regex(^uk|ru$)}/support/success")]
    public IActionResult Success(string culture)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);
        ViewData["Culture"] = culture;
        ViewData["Seo"] = new PageSeoData
        {
            Title  = culture == "uk" ? "Звернення надіслано" : "Обращение отправлено",
            Robots = "noindex, nofollow"
        };
        return View();
    }
}
