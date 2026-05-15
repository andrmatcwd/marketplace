using Marketplace.Modules.Listings.Application.Catalog.Services;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetById;

public sealed class GetReviewByIdHandler(ICatalogDataService data)
    : IRequestHandler<GetReviewByIdQuery, ReviewDto>
{
    public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        => await data.GetAdminReviewByIdAsync(request.Id, cancellationToken) ?? new ReviewDto();
}
