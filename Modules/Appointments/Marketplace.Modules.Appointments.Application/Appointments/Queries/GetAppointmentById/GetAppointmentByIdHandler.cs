using Marketplace.Modules.Appointments.Application.Appointments.Dtos;
using Marketplace.Modules.Appointments.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Queries.GetAppointmentById;

internal sealed class GetAppointmentByIdHandler : IRequestHandler<GetAppointmentByIdQuery, AppointmentDto?>
{
    private readonly IAppointmentRepository _repository;

    public GetAppointmentByIdHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<AppointmentDto?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return a is null ? null : ToDto(a);
    }

    internal static AppointmentDto ToDto(Domain.Entities.Appointment a) => new()
    {
        Id           = a.Id,
        BusinessName = a.BusinessName,
        ContactName  = a.ContactName,
        Phone        = a.Phone,
        Email        = a.Email,
        CategoryName = a.CategoryName,
        CityName     = a.CityName,
        Address      = a.Address,
        Website      = a.Website,
        Description  = a.Description,
        Status       = a.Status,
        AdminNotes   = a.AdminNotes,
        CreatedAtUtc = a.CreatedAtUtc,
        UpdatedAtUtc = a.UpdatedAtUtc
    };
}
