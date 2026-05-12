using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.ExpireSubscriptions;

public sealed class ExpireSubscriptionsHandler : IRequestHandler<ExpireSubscriptionsCommand, Unit>
{
    private readonly IListingSubscriptionService _service;

    public ExpireSubscriptionsHandler(IListingSubscriptionService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(ExpireSubscriptionsCommand request, CancellationToken cancellationToken)
    {
        await _service.ExpireOverdueAsync(cancellationToken);
        return Unit.Value;
    }
}
