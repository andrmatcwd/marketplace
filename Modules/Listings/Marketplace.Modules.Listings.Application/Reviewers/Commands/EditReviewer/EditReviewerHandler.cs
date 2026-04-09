using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.EditReviewer;

public sealed class EditReviewerHandler : IRequestHandler<EditReviewerCommand, int>
{
    public Task<int> Handle(EditReviewerCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
