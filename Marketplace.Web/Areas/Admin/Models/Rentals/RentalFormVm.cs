using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Rentals;

public class RentalFormVm
{
    public int ListingId { get; set; }

    [Display(Name = "Ціна")]
    public string? Price { get; set; }

    [Display(Name = "Кімнати")]
    public string? Rooms { get; set; }

    [Display(Name = "Площа")]
    public string? Area { get; set; }

    [Display(Name = "Поверх")]
    public string? Floor { get; set; }

    [Display(Name = "Особливості (кожна з нового рядка)")]
    public string? FeaturesText { get; set; }
}
