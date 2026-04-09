using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.CreateReviewer;

public sealed class CreateReviewerHandler : IRequestHandler<CreateReviewerCommand, int>
{
    public Task<int> Handle(CreateReviewerCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
