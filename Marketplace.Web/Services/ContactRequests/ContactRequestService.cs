using Marketplace.Modules.Listings.Application.Catalog.Queries;
using Marketplace.Web.Models.Api;
using Marketplace.Web.Services.Notifications;
using MediatR;

namespace Marketplace.Web.Services.ContactRequests;

public sealed class ContactRequestService : IContactRequestService
{
    private readonly IMediator _mediator;
    private readonly IContactNotificationService _notificationService;
    private readonly ILogger<ContactRequestService> _logger;

    public ContactRequestService(
        IMediator mediator,
        IContactNotificationService notificationService,
        ILogger<ContactRequestService> logger)
    {
        _mediator = mediator;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<ContactRequestResult> CreateAsync(
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        var listing = await _mediator.Send(new GetListingDetailsQuery(request.ListingId), cancellationToken);

        if (listing is null)
        {
            return new ContactRequestResult { Success = false, Message = "Listing not found." };
        }

        try
        {
            await _notificationService.NotifyContactRequestAsync(listing, request, cancellationToken);
            return new ContactRequestResult { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process contact request for listing {ListingId}", request.ListingId);
            return new ContactRequestResult { Success = false, Message = "Failed to send message." };
        }
    }
}
