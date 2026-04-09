using Marketplace.Modules.Listings.Application.Appointments.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Queries.GetById;

public sealed class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto>
{
    public Task<AppointmentDto> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AppointmentDto());
    }
}
