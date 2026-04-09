using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.CreateAppointment;

public sealed class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    public Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
