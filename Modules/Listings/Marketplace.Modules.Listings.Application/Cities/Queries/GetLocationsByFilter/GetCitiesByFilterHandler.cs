using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using MediatR;
using Marketplace.Modules.Listings.Application.Services;

namespace Marketplace.Modules.Listings.Application.Cities.Queries.GetCitiesByFilter;

public sealed class GetCitiesByFilterHandler
    : IRequestHandler<GetCitiesByFilterQuery, PagedResult<CityDto>>
{
    private readonly ICityService _cityService;

    public GetCitiesByFilterHandler(ICityService cityService)
    {
        _cityService = cityService;
    }
    
    public Task<PagedResult<CityDto>> Handle(GetCitiesByFilterQuery request, CancellationToken cancellationToken)
    {
        return _cityService.GetByFilterAsync(request.Filter, cancellationToken);
    }
}
