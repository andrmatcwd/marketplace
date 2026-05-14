using Marketplace.Modules.Appointments.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.UpdateAppointment;

internal sealed class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _repository;

    public UpdateAppointmentHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (appointment is null) return false;

        appointment.Status      = request.Status;
        appointment.AdminNotes  = request.AdminNotes;
        appointment.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(appointment);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
