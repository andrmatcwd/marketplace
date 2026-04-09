using Marketplace.Modules.Listings.Application.Reviewers.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Queries.GetById;

public sealed record GetReviewerByIdQuery(int Id) : IRequest<ReviewerDto>;
