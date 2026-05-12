using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.CreateSubscriptionPlan;

public sealed record CreateSubscriptionPlanCommand(
    string Name,
    string? Description,
    int SubscriptionType,
    decimal PriceUah,
    int DurationDays,
    bool IsActive,
    int DisplayOrder
) : IRequest<int>;
