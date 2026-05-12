using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Filters;

public class SubscriptionFilter
{
    public int? ListingId { get; set; }
    public int? PlanId { get; set; }
    public SubscriptionStatus? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
