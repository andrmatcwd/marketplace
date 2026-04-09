using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.CreateReview;

public sealed class CreateReviewHandler : IRequestHandler<CreateReviewCommand, int>
{
    public Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
