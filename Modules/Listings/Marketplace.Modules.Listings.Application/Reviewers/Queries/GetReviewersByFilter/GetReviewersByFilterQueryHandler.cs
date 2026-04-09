using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviewers.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Queries.GetReviewersByFilter;

public sealed class GetReviewersByFilterQueryHandler : IRequestHandler<GetReviewersByFilterQuery, PagedResult<ReviewerDto>>
{
    public Task<PagedResult<ReviewerDto>> Handle(GetReviewersByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<ReviewerDto>());
    }
}
