using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetNotificationById;

public sealed class GetNotificationByIdHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
    private readonly INotificationService _service;

    public GetNotificationByIdHandler(INotificationService service) => _service = service;

    public Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
