using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetListingsByFilter;

public sealed record GetListingsByFilterQuery(ListingFilter Filter) : IRequest<PagedResult<ListingDto>>;
