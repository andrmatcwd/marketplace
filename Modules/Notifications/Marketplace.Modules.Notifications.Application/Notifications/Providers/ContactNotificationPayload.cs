namespace Marketplace.Modules.Notifications.Application.Notifications.Providers;

public sealed class ContactNotificationPayload
{
    public int ListingId { get; init; }
    public string ListingTitle { get; init; } = string.Empty;
    public string? ListingPhone { get; init; }
    public string? ListingEmail { get; init; }
    public string? ListingAddress { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public string CustomerMessage { get; init; } = string.Empty;
}
