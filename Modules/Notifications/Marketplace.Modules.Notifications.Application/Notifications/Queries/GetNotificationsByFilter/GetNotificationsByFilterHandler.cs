using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Application.Services;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetNotificationsByFilter;

public sealed class GetNotificationsByFilterHandler : IRequestHandler<GetNotificationsByFilterQuery, PagedResult<NotificationDto>>
{
    private readonly INotificationService _service;

    public GetNotificationsByFilterHandler(INotificationService service) => _service = service;

    public Task<PagedResult<NotificationDto>> Handle(GetNotificationsByFilterQuery request, CancellationToken cancellationToken)
        => _service.GetByFilterAsync(request.Filter, cancellationToken);
}
