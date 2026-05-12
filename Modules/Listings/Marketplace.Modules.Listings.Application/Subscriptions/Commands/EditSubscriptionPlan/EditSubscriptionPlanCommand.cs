using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.EditSubscriptionPlan;

public sealed record EditSubscriptionPlanCommand(
    int Id,
    string Name,
    string? Description,
    int SubscriptionType,
    decimal PriceUah,
    int DurationDays,
    bool IsActive,
    int DisplayOrder
) : IRequest<Unit>;
