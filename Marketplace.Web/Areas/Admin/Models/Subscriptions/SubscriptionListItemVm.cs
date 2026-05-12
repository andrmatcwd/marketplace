using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Areas.Admin.Models.Subscriptions;

public class SubscriptionListItemVm
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public string PlanName { get; set; } = string.Empty;
    public SubscriptionType SubscriptionType { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public SubscriptionStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
