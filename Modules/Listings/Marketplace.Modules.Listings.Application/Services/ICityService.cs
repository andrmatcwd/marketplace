using Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Services;

public interface ICityService
{
    Task<CityDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<CityDto> GetBySlagAsync(string slag, CancellationToken cancellationToken);

    Task<PagedResult<CityDto>> GetByFilterAsync(CityFilter filter, CancellationToken cancellationToken);

    Task AddAsync(CreateCityCommand command, CancellationToken cancellationToken);

    Task EditAsync(EditCityCommand command, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
