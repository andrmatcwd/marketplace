using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.CancelSubscription;

public sealed class CancelSubscriptionHandler : IRequestHandler<CancelSubscriptionCommand, Unit>
{
    private readonly IListingSubscriptionService _service;

    public CancelSubscriptionHandler(IListingSubscriptionService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await _service.CancelAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
