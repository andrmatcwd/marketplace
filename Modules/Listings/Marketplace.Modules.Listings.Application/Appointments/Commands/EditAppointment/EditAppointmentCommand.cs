using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.EditAppointment;

public sealed record EditAppointmentCommand(int Id, Guid ListingId, string OrganizerId, string AttendeeId, DateTime ScheduledUtc, int DurationMinutes, Domain.Enums.Appointment.AppointmentStatus Status, Domain.Enums.Appointment.AppointmentMeetingType MeetingType) : IRequest<int>;
