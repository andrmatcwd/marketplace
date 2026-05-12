using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetListingSubscriptions;

public sealed class GetListingSubscriptionsHandler
    : IRequestHandler<GetListingSubscriptionsQuery, IReadOnlyList<ListingSubscriptionDto>>
{
    private readonly IListingSubscriptionService _service;

    public GetListingSubscriptionsHandler(IListingSubscriptionService service)
    {
        _service = service;
    }

    public Task<IReadOnlyList<ListingSubscriptionDto>> Handle(GetListingSubscriptionsQuery request, CancellationToken cancellationToken)
        => _service.GetByListingIdAsync(request.ListingId, cancellationToken);
}
