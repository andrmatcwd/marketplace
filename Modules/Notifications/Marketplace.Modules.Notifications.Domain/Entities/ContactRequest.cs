using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Modules.Notifications.Domain.Entities;

public class ContactRequest
{
    public int Id { get; set; }
    public ContactRequestType Type { get; set; }
    public ContactRequestStatus Status { get; set; } = ContactRequestStatus.New;

    public string CustomerName { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public string? CustomerCompany { get; set; }
    public string Message { get; set; } = string.Empty;

    public int? ListingId { get; set; }
    public string? ListingTitle { get; set; }

    public string? AdminNotes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessedAtUtc { get; set; }
}
