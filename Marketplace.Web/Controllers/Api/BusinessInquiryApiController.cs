using Marketplace.Modules.Notifications.Application.ContactRequests.Commands.CreateContactRequest;
using Marketplace.Modules.Notifications.Application.Notifications.Commands.SendContactNotification;
using Marketplace.Modules.Notifications.Domain.Enums;
using Marketplace.Web.Models.Api;
using Marketplace.Web.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Web.Controllers.Api;

[ApiController]
[Route("{culture:regex(^uk|ru$)}/api/business-inquiry")]
public sealed class BusinessInquiryApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BusinessInquiryApiController> _logger;

    public BusinessInquiryApiController(IMediator mediator, ILogger<BusinessInquiryApiController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(
        string culture,
        [FromBody] BusinessInquiryDto request,
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

        await _mediator.Send(new CreateContactRequestCommand(
            Type: ContactRequestType.BusinessInquiry,
            CustomerName: request.Name,
            CustomerPhone: request.Phone,
            CustomerEmail: request.Email,
            CustomerCompany: request.CompanyName,
            Message: request.Message,
            ListingId: null,
            ListingTitle: null), cancellationToken);

        var company = string.IsNullOrWhiteSpace(request.CompanyName)
            ? string.Empty
            : $" ({request.CompanyName})";

        var emailSuffix = string.IsNullOrWhiteSpace(request.Email)
            ? string.Empty
            : $"\nEmail: {request.Email}";

        try
        {
            await _mediator.Send(new SendContactNotificationCommand(
                ListingId: 0,
                ListingTitle: $"Business Inquiry{company}",
                ListingPhone: null,
                ListingEmail: null,
                ListingAddress: null,
                CustomerName: request.Name,
                CustomerPhone: request.Phone + emailSuffix,
                CustomerMessage: request.Message), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to dispatch business inquiry notification.");
        }

        return Ok(new
        {
            success = true,
            message = culture == "uk"
                ? "Вашу заявку отримано. Ми зв'яжемося з вами найближчим часом."
                : "Your inquiry has been received. We will contact you shortly."
        });
    }
}
