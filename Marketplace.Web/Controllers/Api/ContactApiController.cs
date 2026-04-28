using Marketplace.Web.Models.Api;
using Marketplace.Web.Services.ContactRequests;
using Marketplace.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|en$)}/api/contact")]
public sealed class ContactApiController : ControllerBase
{
    private readonly IContactRequestService _contactRequestService;

    public ContactApiController(IContactRequestService contactRequestService)
    {
        _contactRequestService = contactRequestService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(
        string culture,
        [FromBody] ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        culture = CultureHelper.NormalizeRouteCulture(culture);

        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                message = culture == "uk"
                    ? "Будь ласка, коректно заповніть форму."
                    : "Please fill in the form correctly."
            });
        }

        var result = await _contactRequestService.CreateAsync(request, cancellationToken);

        if (!result.Success)
        {
            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        return Ok(new
        {
            success = true,
            message = culture == "uk"
                ? "Повідомлення успішно надіслано."
                : "Message sent successfully."
        });
    }
}