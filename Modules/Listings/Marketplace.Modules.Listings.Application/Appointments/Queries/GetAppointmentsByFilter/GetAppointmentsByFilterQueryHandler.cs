using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Appointments.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Queries.GetAppointmentsByFilter;

public sealed class GetAppointmentsByFilterQueryHandler : IRequestHandler<GetAppointmentsByFilterQuery, PagedResult<AppointmentDto>>
{
    public Task<PagedResult<AppointmentDto>> Handle(GetAppointmentsByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<AppointmentDto>());
    }
}
