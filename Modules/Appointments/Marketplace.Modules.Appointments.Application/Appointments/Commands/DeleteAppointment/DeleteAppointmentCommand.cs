using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.DeleteAppointment;

public sealed record DeleteAppointmentCommand(int Id) : IRequest<bool>;
