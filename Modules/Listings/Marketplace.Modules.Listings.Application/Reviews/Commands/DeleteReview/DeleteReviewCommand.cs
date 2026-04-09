using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.DeleteReview;

public sealed record DeleteReviewCommand(int Id) : IRequest<int>;
