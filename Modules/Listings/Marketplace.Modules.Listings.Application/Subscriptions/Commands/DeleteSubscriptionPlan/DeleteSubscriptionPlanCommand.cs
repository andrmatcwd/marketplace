using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.DeleteSubscriptionPlan;

public sealed record DeleteSubscriptionPlanCommand(int Id) : IRequest<Unit>;
