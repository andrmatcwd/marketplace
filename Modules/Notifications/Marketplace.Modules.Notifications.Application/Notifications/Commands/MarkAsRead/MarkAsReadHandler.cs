using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.MarkAsRead;

public sealed class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, Unit>
{
    private readonly INotificationService _service;

    public MarkAsReadHandler(INotificationService service) => _service = service;

    public async Task<Unit> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        await _service.MarkAsReadAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
