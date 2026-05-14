using Marketplace.Modules.Notifications.Domain.Enums;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Dtos;

public sealed class ContactRequestDto
{
    public int Id { get; init; }
    public ContactRequestType Type { get; init; }
    public ContactRequestStatus Status { get; init; }

    public string CustomerName { get; init; } = string.Empty;
    public string CustomerPhone { get; init; } = string.Empty;
    public string? CustomerEmail { get; init; }
    public string? CustomerCompany { get; init; }
    public string Message { get; init; } = string.Empty;

    public int? ListingId { get; init; }
    public string? ListingTitle { get; init; }

    public string? AdminNotes { get; init; }

    public DateTime CreatedAtUtc { get; init; }
    public DateTime? ProcessedAtUtc { get; init; }
}
