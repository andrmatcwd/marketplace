using Marketplace.Modules.Appointments.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.UpdateAppointment;

public sealed record UpdateAppointmentCommand(
    int               Id,
    AppointmentStatus Status,
    string?           AdminNotes
) : IRequest<bool>;
