using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviews.Dtos;
using Marketplace.Modules.Listings.Application.Reviews.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviews.Queries.GetReviewsByFilter;

public sealed record GetReviewsByFilterQuery(ReviewFilter Filter) : IRequest<PagedResult<ReviewDto>>;
