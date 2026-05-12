using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Areas.Admin.Models.Subscriptions;

public class SubscriptionIndexVm
{
    public int? ListingId { get; set; }
    public int? PlanId { get; set; }
    public SubscriptionStatus? Status { get; set; }
    public List<SubscriptionListItemVm> Items { get; set; } = new();
    public int TotalCount { get; set; }
}
