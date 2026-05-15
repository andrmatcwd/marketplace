using Marketplace.Modules.Listings.Application.Catalog.Services;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetReviewsByFilter;

public sealed class GetReviewsByFilterQueryHandler(ICatalogDataService data)
    : IRequestHandler<GetReviewsByFilterQuery, PagedResult<ReviewDto>>
{
    public Task<PagedResult<ReviewDto>> Handle(
        GetReviewsByFilterQuery request, CancellationToken cancellationToken)
        => data.GetAdminReviewsAsync(request.Filter, cancellationToken);
}
