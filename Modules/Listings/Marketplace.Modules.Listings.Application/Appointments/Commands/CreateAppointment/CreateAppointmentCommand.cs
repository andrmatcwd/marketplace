using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.CreateAppointment;

public sealed record CreateAppointmentCommand(Guid ListingId, string OrganizerId, string AttendeeId, DateTime ScheduledUtc, int DurationMinutes, Domain.Enums.Appointment.AppointmentStatus Status, Domain.Enums.Appointment.AppointmentMeetingType MeetingType) : IRequest<int>;
