using Marketplace.Modules.Listings.Domain.Enums;

namespace Marketplace.Modules.Listings.Domain.Entities;

public sealed class Appointment : AuditedEntity
{
    public Guid Id { get; set; }

    public string ClientName { get; set; } = string.Empty;
    public string ClientPhoneNumber { get; set; } = string.Empty;
    public string? ClientEmail { get; set; }

    public string ManagerId { get; set; } = string.Empty;

    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string? AddressLine { get; set; }
    public AppointmentMeetingType MeetingType { get; set; }

    public DateTime ScheduledAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.New;

    public string? Notes { get; set; }
    public string? ManagerComment { get; set; }

    public Guid? ListingId { get; set; }
    public Listing? Listing { get; set; }
}
