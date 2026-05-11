using Marketplace.Web.Domain.Entities;
using Marketplace.Web.Models.Api;

namespace Marketplace.Web.Services.Notifications;

public interface IContactNotificationService
{
    Task NotifyContactRequestAsync(
        Listing listing,
        ListingContactRequestDto request,
        CancellationToken cancellationToken);
}