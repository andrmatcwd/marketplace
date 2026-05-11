using Marketplace.Modules.Notifications.Application.Common.Models;
using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using Marketplace.Modules.Notifications.Application.Notifications.Filters;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetNotificationsByFilter;

public sealed record GetNotificationsByFilterQuery(NotificationFilter Filter) : IRequest<PagedResult<NotificationDto>>;
