using AutoMapper;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Repositories;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ListingService : IListingService
{
    private readonly IListingRepository listingRepository;
    private readonly IMapper mapper;

    public ListingService(IListingRepository listingRepository, IMapper mapper)
    {
        this.listingRepository = listingRepository;
        this.mapper = mapper;
    }

    public async Task<PagedResult<ListingDto>> GetListingsAsync(ListingFilter filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await listingRepository.GetListingsAsync(filter, cancellationToken);

        return new PagedResult<ListingDto>
        {
            Items = mapper.Map<IReadOnlyCollection<ListingDto>>(items),
            TotalCount = totalCount
        };
    }
}
