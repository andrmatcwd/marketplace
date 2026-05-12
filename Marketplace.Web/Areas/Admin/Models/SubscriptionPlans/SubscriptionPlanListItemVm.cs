using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Areas.Admin.Models.SubscriptionPlans;

public class SubscriptionPlanListItemVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SubscriptionType SubscriptionType { get; set; }
    public decimal PriceUah { get; set; }
    public int DurationDays { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
}
