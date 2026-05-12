using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.CreateSubscriptionPlan;

public sealed class CreateSubscriptionPlanHandler : IRequestHandler<CreateSubscriptionPlanCommand, int>
{
    private readonly ISubscriptionPlanService _service;

    public CreateSubscriptionPlanHandler(ISubscriptionPlanService service)
    {
        _service = service;
    }

    public Task<int> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        => _service.CreateAsync(
            request.Name,
            request.Description,
            request.SubscriptionType,
            request.PriceUah,
            request.DurationDays,
            request.IsActive,
            request.DisplayOrder,
            cancellationToken);
}
