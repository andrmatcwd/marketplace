using Marketplace.Modules.Notifications.Application.ContactRequests.Filters;
using Marketplace.Modules.Notifications.Domain.Entities;

namespace Marketplace.Modules.Notifications.Application.Repositories;

public interface IContactRequestRepository : IBaseRepository<ContactRequest, int>
{
    Task<(IReadOnlyCollection<ContactRequest> Items, int TotalCount)> GetByFilterAsync(
        ContactRequestFilter filter,
        CancellationToken cancellationToken = default);
}
