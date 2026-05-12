using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class ListingSubscription
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
    public int PlanId { get; set; }
    public SubscriptionPlan Plan { get; set; } = null!;
    public string? AssignedByUserId { get; set; }
    public string? RequestedByUserId { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
