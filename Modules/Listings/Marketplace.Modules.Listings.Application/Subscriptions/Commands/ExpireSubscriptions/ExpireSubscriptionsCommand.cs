using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.ExpireSubscriptions;

public sealed record ExpireSubscriptionsCommand : IRequest<Unit>;
