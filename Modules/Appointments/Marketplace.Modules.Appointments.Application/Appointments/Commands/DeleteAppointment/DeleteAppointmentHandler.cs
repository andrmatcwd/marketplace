using Marketplace.Modules.Appointments.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Appointments.Application.Appointments.Commands.DeleteAppointment;

internal sealed class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, bool>
{
    private readonly IAppointmentRepository _repository;

    public DeleteAppointmentHandler(IAppointmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (appointment is null) return false;

        _repository.Remove(appointment);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
