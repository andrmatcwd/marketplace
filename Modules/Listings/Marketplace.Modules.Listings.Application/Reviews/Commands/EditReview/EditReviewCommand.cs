using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.EditReview;

public sealed record EditReviewCommand(int Id, int ListingId, int ReviewerId, int Rating, string Comment) : IRequest<int>;
