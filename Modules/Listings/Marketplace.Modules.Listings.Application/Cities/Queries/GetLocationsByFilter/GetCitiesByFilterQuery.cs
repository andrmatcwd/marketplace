using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;

public sealed record GetCitiesByFilterQuery(CityFilter Filter)
    : IRequest<PagedResult<CityDto>>;
