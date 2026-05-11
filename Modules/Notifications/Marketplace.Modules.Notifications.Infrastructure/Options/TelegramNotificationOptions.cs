namespace Marketplace.Modules.Notifications.Infrastructure.Options;

public sealed class TelegramNotificationOptions
{
    public bool Enabled { get; set; }
    public string BotToken { get; set; } = string.Empty;
    public string ChatId { get; set; } = string.Empty;
}
