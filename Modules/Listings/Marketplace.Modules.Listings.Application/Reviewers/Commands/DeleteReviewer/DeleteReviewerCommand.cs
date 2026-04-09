using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Commands.DeleteReviewer;

public sealed record DeleteReviewerCommand(int Id) : IRequest<int>;
