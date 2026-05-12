using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Dtos;

public class ListingSubscriptionDto
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public int PlanId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public SubscriptionType SubscriptionType { get; set; }
    public string? AssignedByUserId { get; set; }
    public string? RequestedByUserId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public SubscriptionStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
