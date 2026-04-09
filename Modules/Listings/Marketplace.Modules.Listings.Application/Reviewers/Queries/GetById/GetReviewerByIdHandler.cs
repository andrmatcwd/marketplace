using Marketplace.Modules.Listings.Application.Reviewers.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Queries.GetById;

public sealed class GetReviewerByIdHandler : IRequestHandler<GetReviewerByIdQuery, ReviewerDto>
{
    public Task<ReviewerDto> Handle(GetReviewerByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ReviewerDto());
    }
}
