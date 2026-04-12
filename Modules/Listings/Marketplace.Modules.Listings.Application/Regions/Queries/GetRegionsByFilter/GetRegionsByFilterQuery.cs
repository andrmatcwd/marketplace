using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Application.Regions.Filters;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetRegionsByFilter;

public sealed record GetRegionsByFilterQuery(RegionFilter Filter)
    : IRequest<PagedResult<RegionDto>>;
