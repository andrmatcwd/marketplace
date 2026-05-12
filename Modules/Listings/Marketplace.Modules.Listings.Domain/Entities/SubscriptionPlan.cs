using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Domain.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public decimal PriceUah { get; set; }
    public int DurationDays { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<ListingSubscription> Subscriptions { get; set; } = new List<ListingSubscription>();
}
