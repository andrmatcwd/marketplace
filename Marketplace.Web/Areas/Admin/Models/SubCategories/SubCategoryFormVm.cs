using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.SubCategories;

public class SubCategoryFormVm
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Категорія")]
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Опис")]
    public string? Description { get; set; }
}