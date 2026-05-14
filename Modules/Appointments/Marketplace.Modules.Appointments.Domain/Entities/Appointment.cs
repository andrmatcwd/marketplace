using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Modules.Appointments.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }

    public string BusinessName { get; set; } = string.Empty;
    public string ContactName  { get; set; } = string.Empty;
    public string Phone        { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;

    public string  CategoryName { get; set; } = string.Empty;
    public string  CityName     { get; set; } = string.Empty;
    public string? Address      { get; set; }
    public string? Website      { get; set; }
    public string? Description  { get; set; }

    public AppointmentStatus Status     { get; set; } = AppointmentStatus.New;
    public string?           AdminNotes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
