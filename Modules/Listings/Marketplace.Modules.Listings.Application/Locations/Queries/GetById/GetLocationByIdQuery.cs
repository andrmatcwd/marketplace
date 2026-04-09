using Marketplace.Modules.Listings.Application.Locations.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Queries.GetById;

public sealed record GetLocationByIdQuery(int Id) : IRequest<LocationDto>;
