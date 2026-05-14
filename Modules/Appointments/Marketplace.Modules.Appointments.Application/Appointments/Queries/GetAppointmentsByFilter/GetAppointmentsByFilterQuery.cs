using Marketplace.Modules.Appointments.Application.Appointments.Dtos;
using Marketplace.Modules.Appointments.Application.Appointments.Filters;
using Marketplace.Modules.Appointments.Application.Common.Models;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentsByFilter;

public sealed record GetAppointmentsByFilterQuery(AppointmentFilter Filter)
    : IRequest<PagedResult<AppointmentDto>>;
