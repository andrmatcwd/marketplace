using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.DeleteAppointment;

public sealed record DeleteAppointmentCommand(int Id) : IRequest<int>;
