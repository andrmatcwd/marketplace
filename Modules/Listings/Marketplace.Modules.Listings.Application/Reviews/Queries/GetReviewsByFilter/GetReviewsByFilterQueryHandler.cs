using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetReviewsByFilter;

public sealed class GetReviewsByFilterQueryHandler : IRequestHandler<GetReviewsByFilterQuery, PagedResult<ReviewDto>>
{
    public Task<PagedResult<ReviewDto>> Handle(GetReviewsByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<ReviewDto>());
    }
}
