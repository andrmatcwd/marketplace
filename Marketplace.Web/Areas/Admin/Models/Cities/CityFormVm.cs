using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Cities;

public class CityFormVm
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Регіон")]
    public int RegionId { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = string.Empty;
}