using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Areas.Admin.Models.Vacancies;

public class VacancyFormVm
{
    public int? Id { get; set; }

    public int ListingId { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Display(Name = "Тип зайнятості")]
    public string? EmploymentType { get; set; }

    [Display(Name = "Зарплата")]
    public string? SalaryText { get; set; }

    [Display(Name = "Місцезнаходження")]
    public string? LocationText { get; set; }
}
