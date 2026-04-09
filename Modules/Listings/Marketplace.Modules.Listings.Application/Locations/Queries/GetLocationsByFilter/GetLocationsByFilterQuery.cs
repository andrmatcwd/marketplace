using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Locations.Dtos;
using Marketplace.Modules.Listings.Application.Locations.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Queries.GetLocationsByFilter;

public sealed record GetLocationsByFilterQuery(LocationFilter Filter) : IRequest<PagedResult<LocationDto>>;
