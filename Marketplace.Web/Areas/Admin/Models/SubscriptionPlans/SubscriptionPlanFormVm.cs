using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Web.Areas.Admin.Models.SubscriptionPlans;

public class SubscriptionPlanFormVm
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Tier")]
    public SubscriptionType SubscriptionType { get; set; } = SubscriptionType.Basic;

    [Required]
    [Range(0, 999999)]
    [Display(Name = "Price (UAH)")]
    public decimal PriceUah { get; set; }

    [Required]
    [Range(1, 3650)]
    [Display(Name = "Duration (days)")]
    public int DurationDays { get; set; } = 30;

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Display order")]
    public int DisplayOrder { get; set; }
}
