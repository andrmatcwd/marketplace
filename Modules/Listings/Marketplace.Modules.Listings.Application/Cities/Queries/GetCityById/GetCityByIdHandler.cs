using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetCityById;

public sealed class GetCityByIdHandler
    : IRequestHandler<GetCityByIdQuery, CityDto>
{
    private readonly ICityService _cityService;

    public GetCityByIdHandler(ICityService cityService)
    {
        _cityService = cityService;
    }
    
    public Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        return _cityService.GetByIdAsync(request.Id, cancellationToken);
    }
}
