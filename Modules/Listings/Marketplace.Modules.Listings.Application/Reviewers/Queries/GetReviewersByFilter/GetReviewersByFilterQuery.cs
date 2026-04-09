using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Reviewers.Dtos;
using Marketplace.Modules.Listings.Application.Reviewers.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Reviewers.Queries.GetReviewersByFilter;

public sealed record GetReviewersByFilterQuery(ReviewerFilter Filter) : IRequest<PagedResult<ReviewerDto>>;
