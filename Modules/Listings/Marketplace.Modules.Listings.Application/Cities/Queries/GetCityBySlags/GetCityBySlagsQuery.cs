using Marketplace.Modules.Listings.Application.Cities.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetCityBySlags;

public sealed record GetCityBySlagsQuery(
    string CitySlag
) : IRequest<CityDto>;
