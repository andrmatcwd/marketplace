using Marketplace.Modules.Notifications.Application.Notifications.Dtos;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetNotificationById;

public sealed record GetNotificationByIdQuery(int Id) : IRequest<NotificationDto?>;
