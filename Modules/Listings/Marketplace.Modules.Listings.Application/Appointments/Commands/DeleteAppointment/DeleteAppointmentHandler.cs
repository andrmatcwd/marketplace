using MediatR;

namespace Marketplace.Modules.Listings.Application.Appointments.Commands.DeleteAppointment;

public sealed class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentCommand, int>
{
    public Task<int> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
