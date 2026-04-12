using System;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;
using Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Application.Regions.Filters;

namespace Marketplace.Modules.Listings.Application.Services;

public interface IRegionService
{
    Task<RegionDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<PagedResult<RegionDto>> GetByFilterAsync(RegionFilter filter, CancellationToken cancellationToken);

    Task AddAsync(CreateRegionCommand command, CancellationToken cancellationToken);

    Task EditAsync(EditRegionCommand command, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
