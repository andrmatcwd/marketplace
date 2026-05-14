using Marketplace.Modules.Appointments.Application.Repositories;
using Marketplace.Modules.Appointments.Domain.Entities;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.CreateAppointment;

internal sealed class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly IAppointmentRepository _repository;

    public CreateAppointmentHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment
        {
            BusinessName = request.BusinessName,
            ContactName  = request.ContactName,
            Phone        = request.Phone,
            Email        = request.Email,
            CategoryName = request.CategoryName,
            CityName     = request.CityName,
            Address      = request.Address,
            Website      = request.Website,
            Description  = request.Description,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(appointment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return appointment.Id;
    }
}
