using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.EditReview;

public sealed class EditReviewHandler : IRequestHandler<EditReviewCommand, int>
{
    public Task<int> Handle(EditReviewCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Id);
    }
}
