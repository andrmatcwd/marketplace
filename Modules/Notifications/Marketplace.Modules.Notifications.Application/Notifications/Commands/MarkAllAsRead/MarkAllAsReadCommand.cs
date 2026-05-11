using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.MarkAllAsRead;

public sealed record MarkAllAsReadCommand(string RecipientId) : IRequest<Unit>;
