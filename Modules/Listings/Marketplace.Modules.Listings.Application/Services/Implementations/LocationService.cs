using System;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class LocationService : ILocationService
{
    private readonly ILocationRepository locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }
}
