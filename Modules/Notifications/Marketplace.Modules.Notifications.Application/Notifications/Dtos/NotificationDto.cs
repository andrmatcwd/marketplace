using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Modules.Notifications.Application.Notifications.Dtos;

public class NotificationDto
{
    public int Id { get; set; }
    public string RecipientId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string? Url { get; set; }
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
