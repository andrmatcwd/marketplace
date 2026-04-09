using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.DeleteReviewer;

public sealed class DeleteReviewerHandler : IRequestHandler<DeleteReviewerCommand, int>
{
    public Task<int> Handle(DeleteReviewerCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
