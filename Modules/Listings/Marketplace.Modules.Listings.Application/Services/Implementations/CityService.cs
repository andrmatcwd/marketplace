using System;
using AutoMapper;
using Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;
using Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;
using Marketplace.Modules.Listings.Application.Cities.Dtos;
using Marketplace.Modules.Listings.Application.Cities.Filters;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public CityService(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<CityDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(id, cancellationToken);

        if (city == null)
        {
            throw new Exception($"City with id {id} not found.");
        }

        return _mapper.Map<CityDto>(city);
    }

    public async Task<PagedResult<CityDto>> GetByFilterAsync(CityFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _cityRepository.GetByFilterAsync(filter, cancellationToken);

        return new PagedResult<CityDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<CityDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(CreateCityCommand command, CancellationToken cancellationToken)
    {
        var city = new City
        {
            Name = command.Name,
            RegionId = command.RegionId
        };

        await _cityRepository.AddAsync(city, cancellationToken);

        await _cityRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditCityCommand command, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(command.Id, cancellationToken);

        if (city == null)
        {
            throw new Exception($"City with id {command.Id} not found.");
        }

        city.Name = command.Name;
        city.RegionId = command.RegionId;

        _cityRepository.Update(city);
        await _cityRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(id, cancellationToken);

        if (city == null)
        {
            throw new Exception($"City with id {id} not found.");
        }

        _cityRepository.Remove(city);
        await _cityRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<CityDto> GetBySlagAsync(string slag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
