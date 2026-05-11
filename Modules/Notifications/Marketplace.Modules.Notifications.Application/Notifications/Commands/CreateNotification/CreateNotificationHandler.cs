using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.CreateNotification;

public sealed class CreateNotificationHandler : IRequestHandler<CreateNotificationCommand, Unit>
{
    private readonly INotificationService _service;

    public CreateNotificationHandler(INotificationService service) => _service = service;

    public async Task<Unit> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        await _service.AddAsync(request, cancellationToken);
        return Unit.Value;
    }
}
