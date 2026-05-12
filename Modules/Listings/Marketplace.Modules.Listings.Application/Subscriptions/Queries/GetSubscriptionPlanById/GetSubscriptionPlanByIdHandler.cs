using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionPlanById;

public sealed class GetSubscriptionPlanByIdHandler
    : IRequestHandler<GetSubscriptionPlanByIdQuery, SubscriptionPlanDto?>
{
    private readonly ISubscriptionPlanService _service;

    public GetSubscriptionPlanByIdHandler(ISubscriptionPlanService service)
    {
        _service = service;
    }

    public Task<SubscriptionPlanDto?> Handle(GetSubscriptionPlanByIdQuery request, CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
