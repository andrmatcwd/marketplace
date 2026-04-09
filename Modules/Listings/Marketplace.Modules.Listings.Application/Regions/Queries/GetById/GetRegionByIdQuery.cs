using Marketplace.Modules.Listings.Application.Regions.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Queries.GetById;

public sealed record GetRegionByIdQuery(int Id) : IRequest<RegionDto>;
