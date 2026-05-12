using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlans;

public sealed class GetSubscriptionPlansHandler
    : IRequestHandler<GetSubscriptionPlansQuery, IReadOnlyList<SubscriptionPlanDto>>
{
    private readonly ISubscriptionPlanService _service;

    public GetSubscriptionPlansHandler(ISubscriptionPlanService service)
    {
        _service = service;
    }

    public Task<IReadOnlyList<SubscriptionPlanDto>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
        => request.ActiveOnly
            ? _service.GetActiveAsync(cancellationToken)
            : _service.GetAllAsync(cancellationToken);
}
