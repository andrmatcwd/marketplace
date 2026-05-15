using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Models.MyListings;

public class MyListingFormVm
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Short Description")]
    public string? ShortDescription { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [MaxLength(300)]
    [Display(Name = "Address")]
    public string? Address { get; set; }

    [MaxLength(50)]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [MaxLength(120)]
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [MaxLength(300)]
    [Display(Name = "Website")]
    public string? Website { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Subcategory is required")]
    [Display(Name = "Subcategory")]
    public int SubCategoryId { get; set; }

    [Required(ErrorMessage = "City is required")]
    [Display(Name = "City")]
    public int CityId { get; set; }

    [Display(Name = "Latitude")]
    public double? Latitude { get; set; }

    [Display(Name = "Longitude")]
    public double? Longitude { get; set; }
}
