using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, int>
{
    public Task<int> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
