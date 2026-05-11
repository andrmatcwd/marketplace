using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.DeleteNotification;

public sealed record DeleteNotificationCommand(int Id) : IRequest<Unit>;
