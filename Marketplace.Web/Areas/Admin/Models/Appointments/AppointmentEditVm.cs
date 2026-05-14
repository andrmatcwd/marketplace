using System.ComponentModel.DataAnnotations;
using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.Appointments;

public sealed class AppointmentEditVm
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Статус")]
    public AppointmentStatus Status { get; set; }

    [MaxLength(2000)]
    [Display(Name = "Нотатки адміна")]
    public string? AdminNotes { get; set; }
}
