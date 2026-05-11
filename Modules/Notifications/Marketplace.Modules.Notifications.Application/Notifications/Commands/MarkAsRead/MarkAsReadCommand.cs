using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(int Id) : IRequest<Unit>;
