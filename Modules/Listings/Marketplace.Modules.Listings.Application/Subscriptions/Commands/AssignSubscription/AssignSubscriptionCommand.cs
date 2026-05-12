using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.AssignSubscription;

public sealed record AssignSubscriptionCommand(
    int ListingId,
    int PlanId,
    DateTime StartsAt,
    string? AssignedByUserId,
    string? Notes
) : IRequest<Unit>;
