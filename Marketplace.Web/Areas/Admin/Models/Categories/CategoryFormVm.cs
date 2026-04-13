using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Categories;

public class CategoryFormVm
{
    public int? Id { get; set; }

    [Required]
    public int CityId { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Іконка")]
    public string? Icon { get; set; }
}