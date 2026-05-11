using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Web.Models.Api;

namespace Marketplace.Web.Services.Notifications;

public interface IContactNotificationService
{
    Task NotifyContactRequestAsync(
        ListingDetailsDto listing,
        ListingContactRequestDto request,
        CancellationToken cancellationToken);
}
