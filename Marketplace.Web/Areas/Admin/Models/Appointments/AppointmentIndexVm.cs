using Marketplace.Modules.Appointments.Domain.Enums;

namespace Marketplace.Web.Areas.Admin.Models.Appointments;

public sealed class AppointmentIndexVm
{
    public string?            Search  { get; init; }
    public AppointmentStatus? Status  { get; init; }
    public int                Page    { get; init; } = 1;
    public int                TotalCount  { get; init; }
    public int                TotalPages  { get; init; }
    public IReadOnlyCollection<AppointmentListItemVm> Items { get; init; } = [];
}

public sealed class AppointmentListItemVm
{
    public int               Id           { get; init; }
    public string            BusinessName { get; init; } = string.Empty;
    public string            ContactName  { get; init; } = string.Empty;
    public string            Phone        { get; init; } = string.Empty;
    public string            Email        { get; init; } = string.Empty;
    public string            CategoryName { get; init; } = string.Empty;
    public string            CityName     { get; init; } = string.Empty;
    public AppointmentStatus Status       { get; init; }
    public DateTime          CreatedAtUtc { get; init; }
}
