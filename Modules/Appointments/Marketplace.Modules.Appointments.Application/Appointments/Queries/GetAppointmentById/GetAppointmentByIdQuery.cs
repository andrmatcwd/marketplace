using Marketplace.Modules.Appointments.Application.Appointments.Dtos;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(int Id) : IRequest<AppointmentDto?>;
