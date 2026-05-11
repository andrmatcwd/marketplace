namespace Marketplace.Modules.Notifications.Infrastructure.Options;

public sealed class ContactNotificationOptions
{
    public EmailNotificationOptions Email { get; set; } = new();
    public TelegramNotificationOptions Telegram { get; set; } = new();
}
