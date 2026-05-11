namespace Marketplace.Web.Options;

public sealed class ContactNotificationOptions
{
    public EmailNotificationOptions Email { get; set; } = new();
    public TelegramNotificationOptions Telegram { get; set; } = new();
}
