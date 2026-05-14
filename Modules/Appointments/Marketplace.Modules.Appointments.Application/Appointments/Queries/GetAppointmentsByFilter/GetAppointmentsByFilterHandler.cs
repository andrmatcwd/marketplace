using Marketplace.Modules.Appointments.Application.Appointments.Dtos;
using Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentById;
using Marketplace.Modules.Appointments.Application.Common.Models;
using Marketplace.Modules.Appointments.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentsByFilter;

internal sealed class GetAppointmentsByFilterHandler
    : IRequestHandler<GetAppointmentsByFilterQuery, PagedResult<AppointmentDto>>
{
    private readonly IAppointmentRepository _repository;

    public GetAppointmentsByFilterHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<AppointmentDto>> Handle(
        GetAppointmentsByFilterQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await _repository.GetByFilterAsync(request.Filter, cancellationToken);

        return new PagedResult<AppointmentDto>
        {
            Items      = items.Select(GetAppointmentByIdHandler.ToDto).ToList(),
            Page       = request.Filter.Page,
            PageSize   = request.Filter.PageSize,
            TotalCount = total
        };
    }
}
