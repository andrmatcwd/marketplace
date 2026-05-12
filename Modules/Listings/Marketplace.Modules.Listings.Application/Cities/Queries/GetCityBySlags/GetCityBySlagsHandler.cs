using System;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetCityBySlags;

public class GetCityBySlagsHandler
    : IRequestHandler<GetCityBySlagsQuery, CityDto>
{
    private readonly ICityService _cityService;

    public GetCityBySlagsHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public Task<CityDto> Handle(GetCityBySlagsQuery request, CancellationToken cancellationToken)
    {
        return _cityService.GetBySlagAsync(request.CitySlag, cancellationToken);
    }
}
