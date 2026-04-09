using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.CreateReviewer;

public sealed record CreateReviewerCommand(string UserId, int ReviewsCount, double AverageGivenRating, bool IsActive) : IRequest<int>;
