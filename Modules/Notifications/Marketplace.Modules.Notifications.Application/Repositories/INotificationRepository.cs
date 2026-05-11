using Marketplace.Modules.Notifications.Application.Notifications.Filters;
using Marketplace.Modules.Notifications.Domain.Entities;

namespace Marketplace.Modules.Notifications.Application.Repositories;

public interface INotificationRepository : IBaseRepository<Notification, int>
{
    Task<(IReadOnlyCollection<Notification> Items, int TotalCount)> GetByFilterAsync(NotificationFilter filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Notification>> GetByRecipientIdAsync(string recipientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string recipientId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(string recipientId, CancellationToken cancellationToken = default);
}
