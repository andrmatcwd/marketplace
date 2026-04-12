using Marketplace.Modules.Listings.Application.Cities.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetById;

public sealed record GetCityByIdQuery(int Id)
    : IRequest<CityDto>;
