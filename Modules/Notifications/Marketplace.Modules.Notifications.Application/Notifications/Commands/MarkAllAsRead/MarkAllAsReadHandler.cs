using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.MarkAllAsRead;

public sealed class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand, Unit>
{
    private readonly INotificationService _service;

    public MarkAllAsReadHandler(INotificationService service) => _service = service;

    public async Task<Unit> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        await _service.MarkAllAsReadAsync(request.RecipientId, cancellationToken);
        return Unit.Value;
    }
}
