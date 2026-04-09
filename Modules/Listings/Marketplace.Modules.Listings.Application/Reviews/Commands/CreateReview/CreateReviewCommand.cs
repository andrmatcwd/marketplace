using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Commands.CreateReview;

public sealed record CreateReviewCommand(int ListingId, int ReviewerId, int Rating, string Comment) : IRequest<int>;
