using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Modules.Notifications.Application.Notifications.Commands.SendContactNotification;
using Marketplace.Web.Models.Api;
using MediatR;

namespace Marketplace.Web.Services.ContactRequests;

public sealed class ContactRequestService : IContactRequestService
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContactRequestService> _logger;

    public ContactRequestService(IMediator mediator, ILogger<ContactRequestService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<ContactRequestResult> CreateAsync(
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        var listing = await _mediator.Send(new GetListingDetailsQuery(request.ListingId), cancellationToken);

        if (listing is null)
            return new ContactRequestResult { Success = false, Message = "Listing not found." };

        try
        {
            await _mediator.Send(new SendContactNotificationCommand(
                listing.Id,
                listing.Title,
                listing.Phone,
                listing.Email,
                listing.Address,
                request.Name,
                request.Phone,
                request.Message), cancellationToken);

            return new ContactRequestResult { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to dispatch contact notification for listing {ListingId}.", request.ListingId);
            return new ContactRequestResult { Success = false, Message = "Failed to send message." };
        }
    }
}
