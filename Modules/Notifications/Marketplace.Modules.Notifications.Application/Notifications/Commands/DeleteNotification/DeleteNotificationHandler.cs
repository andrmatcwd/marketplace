using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.DeleteNotification;

public sealed class DeleteNotificationHandler : IRequestHandler<DeleteNotificationCommand, Unit>
{
    private readonly INotificationService _service;

    public DeleteNotificationHandler(INotificationService service) => _service = service;

    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
