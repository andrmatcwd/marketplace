using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetUnreadCount;

public sealed class GetUnreadCountHandler : IRequestHandler<GetUnreadCountQuery, int>
{
    private readonly INotificationService _service;

    public GetUnreadCountHandler(INotificationService service) => _service = service;

    public Task<int> Handle(GetUnreadCountQuery request, CancellationToken cancellationToken)
        => _service.GetUnreadCountAsync(request.RecipientId, cancellationToken);
}
