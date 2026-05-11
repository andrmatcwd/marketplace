using Marketplace.Web.Data;
using Marketplace.Web.Models.Api;
using Marketplace.Web.Services.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Web.Services.ContactRequests;

public sealed class ContactRequestService : IContactRequestService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IContactNotificationService _notificationService;
    private readonly ILogger<ContactRequestService> _logger;

    public ContactRequestService(
        ApplicationDbContext dbContext,
        IContactNotificationService notificationService,
        ILogger<ContactRequestService> logger)
    {
        _dbContext = dbContext;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<ContactRequestResult> CreateAsync(
        ListingContactRequestDto request,
        CancellationToken cancellationToken)
    {
        var listing = await _dbContext.Listings
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.IsPublished && x.Id == request.ListingId,
                cancellationToken);

        if (listing is null)
        {
            return new ContactRequestResult
            {
                Success = false,
                Message = "Listing not found."
            };
        }

        try
        {
            await _notificationService.NotifyContactRequestAsync(
                listing,
                request,
                cancellationToken);

            return new ContactRequestResult
            {
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process contact request for listing {ListingId}",
                request.ListingId);

            return new ContactRequestResult
            {
                Success = false,
                Message = "Failed to send message."
            };
        }
    }
}