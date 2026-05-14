using System.ComponentModel.DataAnnotations;

namespace Marketplace.Web.Models.Appointment;

public sealed class AppointmentFormVm
{
    [Required, MaxLength(300)]
    public string BusinessName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ContactName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CategoryName { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CityName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(500), Url]
    public string? Website { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
}
