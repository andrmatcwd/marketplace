using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Modules.Appointments.Application.Appointments.Dtos;

public sealed class AppointmentDto
{
    public int    Id           { get; init; }
    public string BusinessName { get; init; } = string.Empty;
    public string ContactName  { get; init; } = string.Empty;
    public string Phone        { get; init; } = string.Empty;
    public string Email        { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string CityName     { get; init; } = string.Empty;
    public string? Address     { get; init; }
    public string? Website     { get; init; }
    public string? Description { get; init; }
    public AppointmentStatus Status     { get; init; }
    public string?           AdminNotes { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
