using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.CancelSubscription;

public sealed record CancelSubscriptionCommand(int Id) : IRequest<Unit>;
