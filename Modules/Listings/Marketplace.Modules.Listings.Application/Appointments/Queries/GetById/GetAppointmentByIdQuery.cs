using Marketplace.Modules.Listings.Application.Appointments.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Queries.GetById;

public sealed record GetAppointmentByIdQuery(int Id) : IRequest<AppointmentDto>;
