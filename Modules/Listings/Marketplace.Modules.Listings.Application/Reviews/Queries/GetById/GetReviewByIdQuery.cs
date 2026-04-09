using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetById;

public sealed record GetReviewByIdQuery(int Id) : IRequest<ReviewDto>;
