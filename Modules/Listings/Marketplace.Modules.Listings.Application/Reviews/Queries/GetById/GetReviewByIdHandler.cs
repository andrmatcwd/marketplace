using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetById;

public sealed class GetReviewByIdHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
{
    public Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ReviewDto());
    }
}
