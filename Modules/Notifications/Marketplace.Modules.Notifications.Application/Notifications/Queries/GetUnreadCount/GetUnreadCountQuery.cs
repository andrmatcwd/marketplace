using MediatR;

namespace Marketplace.Modules.Notifications.Application.Notifications.Queries.GetUnreadCount;

public sealed record GetUnreadCountQuery(string RecipientId) : IRequest<int>;
