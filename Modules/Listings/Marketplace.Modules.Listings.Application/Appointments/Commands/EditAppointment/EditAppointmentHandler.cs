using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.EditAppointment;

public sealed class EditAppointmentHandler : IRequestHandler<EditAppointmentCommand, int>
{
    public Task<int> Handle(EditAppointmentCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
