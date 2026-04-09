using Marketplace.Modules.Listings.Domain.Enums.Appointment;

namespace Marketplace.Modules.Listings.Application.Appointments.Dtos;

public class AppointmentDto
{
    public int Id { get; set; }
    public Guid ListingId { get; set; }
    public string OrganizerId { get; set; } = string.Empty;
    public string AttendeeId { get; set; } = string.Empty;
    public DateTime ScheduledUtc { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentMeetingType MeetingType { get; set; }
}
