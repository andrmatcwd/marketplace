using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Subscriptions;

public class AssignSubscriptionVm
{
    [Required]
    [Display(Name = "Listing")]
    public int ListingId { get; set; }

    [Required]
    [Display(Name = "Plan")]
    public int PlanId { get; set; }

    [Required]
    [Display(Name = "Starts at")]
    [DataType(DataType.Date)]
    public DateTime StartsAt { get; set; } = DateTime.UtcNow.Date;

    [Display(Name = "Notes")]
    public string? Notes { get; set; }
}
