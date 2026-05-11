using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.Notifications.Commands.CreateNotification;
using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Application.Notifications.Filters;

namespace Marketplace.Modules.Notifications.Application.Services;

public interface INotificationService
{
    Task<NotificationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<NotificationDto>> GetByFilterAsync(NotificationFilter filter, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<NotificationDto>> GetByRecipientIdAsync(string recipientId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string recipientId, CancellationToken cancellationToken = default);
    Task AddAsync(CreateNotificationCommand command, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(int id, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(string recipientId, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
