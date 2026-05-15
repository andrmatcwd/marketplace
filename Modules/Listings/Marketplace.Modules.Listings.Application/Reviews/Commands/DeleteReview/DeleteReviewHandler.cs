using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewHandler(ICatalogDataService data)
    : IRequestHandler<DeleteReviewCommand, int>
{
    public async Task<int> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        await data.DeleteReviewAndRecalculateAsync(request.Id, cancellationToken);
        return request.Id;
    }
}
