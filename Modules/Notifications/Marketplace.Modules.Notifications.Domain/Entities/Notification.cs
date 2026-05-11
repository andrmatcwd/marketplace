using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Modules.Notifications.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string? Url { get; set; }
    public NotificationType Type { get; set; } = NotificationType.Info;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
