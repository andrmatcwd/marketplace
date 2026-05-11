namespace Marketplace.Modules.Notifications.Application.Notifications.Providers;

public interface INotificationProvider
{
    Task SendAsync(ContactNotificationPayload payload, CancellationToken cancellationToken);
}
