using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.EditReviewer;

public sealed record EditReviewerCommand(int Id, string UserId, int ReviewsCount, double AverageGivenRating, bool IsActive) : IRequest<int>;
