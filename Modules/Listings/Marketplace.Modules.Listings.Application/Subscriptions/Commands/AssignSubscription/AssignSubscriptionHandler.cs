using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.AssignSubscription;

public sealed class AssignSubscriptionHandler : IRequestHandler<AssignSubscriptionCommand, Unit>
{
    private readonly IListingSubscriptionService _service;

    public AssignSubscriptionHandler(IListingSubscriptionService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(AssignSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await _service.AssignAsync(
            request.ListingId,
            request.PlanId,
            request.StartsAt,
            request.AssignedByUserId,
            request.Notes,
            cancellationToken);

        return Unit.Value;
    }
}
