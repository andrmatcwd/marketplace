using Marketplace.Modules.Listings.Domain.Enums.Appointment;

namespace Marketplace.Modules.Listings.Application.Appointments.Filters;

public sealed class AppointmentFilter
{
    public int? ListingId { get; set; }
    public string? OrganizerId { get; set; }
    public string? AttendeeId { get; set; }
    AppointmentStatus? Status { get; set; }
    AppointmentMeetingType? MeetingType { get; set; }
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
}
