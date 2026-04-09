using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class RegionService : IRegionService
{
    private readonly IRegionRepository regionRepository;

    public RegionService(IRegionRepository regionRepository)
    {
        this.regionRepository = regionRepository;
    }
}
