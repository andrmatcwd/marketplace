using AutoMapper;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;
using Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;
using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Listings.Filters;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class ListingService : IListingService
{
    private readonly IListingRepository _listingRepository;
    private readonly IMapper _mapper;

    public ListingService(IListingRepository listingRepository, IMapper mapper)
    {
        _listingRepository = listingRepository;
        _mapper = mapper;
    }

    public async Task<ListingDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);

        if (listing == null)
        {
            throw new Exception($"Listing with id {id} not found.");
        }

        return _mapper.Map<ListingDto>(listing);
    }

    public Task<ListingDto> GetBySlagsAsync(
        string citySlag,
        string categorySlag,
        string subCategorySlag,
        string slag,
        int id,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<ListingDto>> GetByFilterAsync(ListingFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _listingRepository.GetByFilterAsync(filter, cancellationToken);

        return new PagedResult<ListingDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<ListingDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(CreateListingCommand command, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Title = command.Title,
            Description = command.Description
        };

        await _listingRepository.AddAsync(listing, cancellationToken);

        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditListingCommand command, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(command.Id, cancellationToken);

        if (listing == null)
        {
            throw new Exception($"Listing with id {command.Id} not found.");
        }

        listing.Title = command.Title;
        listing.Description = command.Description;

        _listingRepository.Update(listing);
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);

        if (listing == null)
        {
            throw new Exception($"Listing with id {id} not found.");
        }

        _listingRepository.Remove(listing);
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }
}
