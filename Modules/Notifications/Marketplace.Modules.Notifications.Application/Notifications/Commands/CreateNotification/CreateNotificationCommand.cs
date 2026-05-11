using Marketplace.Modules.Notifications.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.CreateNotification;

public sealed record CreateNotificationCommand(
    string RecipientId,
    string Title,
    string? Body,
    string? Url,
    NotificationType Type
) : IRequest<Unit>;
