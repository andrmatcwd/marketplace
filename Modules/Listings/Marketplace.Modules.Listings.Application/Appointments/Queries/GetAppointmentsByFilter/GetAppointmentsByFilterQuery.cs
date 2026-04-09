using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Appointments.Dtos;
using Marketplace.Modules.Listings.Application.Appointments.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Queries.GetAppointmentsByFilter;

public sealed record GetAppointmentsByFilterQuery(AppointmentFilter Filter) : IRequest<PagedResult<AppointmentDto>>;
