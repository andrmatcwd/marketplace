using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Cities;

public class CityFormVm
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Slug (авто якщо порожній)")]
    public string? Slug { get; set; }

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Опубліковано")]
    public bool IsPublished { get; set; } = true;

    [Display(Name = "Порядок сортування")]
    public int SortOrder { get; set; }
}
