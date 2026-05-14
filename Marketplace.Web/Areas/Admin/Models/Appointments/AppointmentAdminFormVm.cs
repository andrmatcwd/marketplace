using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.Appointments;

public sealed class AppointmentAdminFormVm
{
    [Required, MaxLength(300)]
    [Display(Name = "Назва компанії")]
    public string BusinessName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    [Display(Name = "Ім'я контакту")]
    public string ContactName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    [Display(Name = "Телефон")]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    [Display(Name = "Сфера діяльності")]
    public string CategoryName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    [Display(Name = "Місто")]
    public string CityName { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Адреса")]
    public string? Address { get; set; }

    [MaxLength(500)]
    [Display(Name = "Сайт")]
    public string? Website { get; set; }

    [MaxLength(2000)]
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Статус")]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.New;

    [MaxLength(2000)]
    [Display(Name = "Нотатки адміна")]
    public string? AdminNotes { get; set; }
}
