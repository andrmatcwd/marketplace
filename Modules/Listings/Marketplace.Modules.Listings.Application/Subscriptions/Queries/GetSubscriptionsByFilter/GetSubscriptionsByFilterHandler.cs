using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Queries.GetSubscriptionsByFilter;

public sealed class GetSubscriptionsByFilterHandler
    : IRequestHandler<GetSubscriptionsByFilterQuery, PagedResult<ListingSubscriptionDto>>
{
    private readonly IListingSubscriptionService _service;

    public GetSubscriptionsByFilterHandler(IListingSubscriptionService service)
    {
        _service = service;
    }

    public Task<PagedResult<ListingSubscriptionDto>> Handle(GetSubscriptionsByFilterQuery request, CancellationToken cancellationToken)
        => _service.GetByFilterAsync(request.Filter, cancellationToken);
}
