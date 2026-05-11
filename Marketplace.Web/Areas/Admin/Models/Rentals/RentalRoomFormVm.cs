using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Rentals;

public class RentalRoomFormVm
{
    public int? Id { get; set; }

    public int RentalId { get; set; }

    public int ListingId { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Ціна")]
    public string? Price { get; set; }

    [Display(Name = "Площа")]
    public string? Area { get; set; }

    [Display(Name = "Гості")]
    public string? Guests { get; set; }

    [Display(Name = "Ліжка")]
    public string? Beds { get; set; }

    [Display(Name = "Зручності (кожна з нового рядка)")]
    public string? AmenitiesText { get; set; }
}
