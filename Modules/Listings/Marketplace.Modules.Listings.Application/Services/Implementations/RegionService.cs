using System;
using AutoMapper;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;
using Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;
using Marketplace.Modules.Listings.Application.Regions.Dtos;
using Marketplace.Modules.Listings.Application.Regions.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class RegionService : IRegionService
{
    private readonly IRegionRepository _regionRepository;
    private readonly IMapper _mapper;

    public RegionService(IRegionRepository regionRepository, IMapper mapper)
    {
        _regionRepository = regionRepository;
        _mapper = mapper;
    }

        public async Task<RegionDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var region = await _regionRepository.GetByIdAsync(id, cancellationToken);

        if (region == null)
        {
            throw new Exception($"Region with id {id} not found.");
        }

        return _mapper.Map<RegionDto>(region);
    }

    public async Task<PagedResult<RegionDto>> GetByFilterAsync(RegionFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _regionRepository.GetByFilterAsync(filter, cancellationToken);

        return new PagedResult<RegionDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<RegionDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(CreateRegionCommand command, CancellationToken cancellationToken)
    {
        var region = new Region
        {
            Name = command.Name
        };

        await _regionRepository.AddAsync(region, cancellationToken);

        await _regionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditRegionCommand command, CancellationToken cancellationToken)
    {
        var region = await _regionRepository.GetByIdAsync(command.Id, cancellationToken);

        if (region == null)
        {
            throw new Exception($"Region with id {command.Id} not found.");
        }

        region.Name = command.Name;

        _regionRepository.Update(region);
        await _regionRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var region = await _regionRepository.GetByIdAsync(id, cancellationToken);

        if (region == null)
        {
            throw new Exception($"Region with id {id} not found.");
        }

        _regionRepository.Remove(region);
        await _regionRepository.SaveChangesAsync(cancellationToken);
    }
}
