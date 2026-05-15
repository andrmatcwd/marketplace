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
    private readonly ISlugService _slugService;
    private readonly IMapper _mapper;

    public ListingService(IListingRepository listingRepository, ISlugService slugService, IMapper mapper)
    {
        _listingRepository = listingRepository;
        _slugService = slugService;
        _mapper = mapper;
    }

    public async Task<ListingDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);
        if (listing == null) throw new Exception($"Listing with id {id} not found.");
        return _mapper.Map<ListingDto>(listing);
    }

    public Task<ListingDto> GetBySlagsAsync(string citySlag, string categorySlag, string subCategorySlag, string slag, int id, CancellationToken cancellationToken)
        => throw new NotImplementedException();

    public async Task<PagedResult<ListingDto>> GetByFilterAsync(ListingFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _listingRepository.GetByFilterAsync(filter, cancellationToken);
        return new PagedResult<ListingDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<ListingDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task<int> AddAsync(CreateListingCommand command, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Title = command.Title,
            Slug = string.IsNullOrWhiteSpace(command.Slug)
                ? _slugService.Generate(command.Title)
                : command.Slug.Trim().ToLowerInvariant(),
            ShortDescription = command.ShortDescription,
            Description = command.Description,
            Phone = command.Phone,
            Email = command.Email,
            Website = command.Website,
            Address = command.Address,
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            SellerId = command.SellerId,
            SubscriptionType = command.SubscriptionType,
            Status = command.Status,
            CategoryId = command.CategoryId,
            SubCategoryId = command.SubCategoryId,
            CityId = command.CityId
        };

        await _listingRepository.AddAsync(listing, cancellationToken);
        await _listingRepository.SaveChangesAsync(cancellationToken);
        return listing.Id;
    }

    public async Task EditAsync(EditListingCommand command, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(command.Id, cancellationToken);
        if (listing == null) throw new Exception($"Listing with id {command.Id} not found.");

        listing.Title = command.Title;
        listing.Slug = string.IsNullOrWhiteSpace(command.Slug)
            ? _slugService.Generate(command.Title)
            : command.Slug.Trim().ToLowerInvariant();
        listing.ShortDescription = command.ShortDescription;
        listing.Description = command.Description;
        listing.Phone = command.Phone;
        listing.Email = command.Email;
        listing.Website = command.Website;
        listing.Address = command.Address;
        listing.Latitude = command.Latitude;
        listing.Longitude = command.Longitude;
        listing.SellerId = command.SellerId;
        listing.SubscriptionType = command.SubscriptionType;
        listing.Status = command.Status;
        listing.CategoryId = command.CategoryId;
        listing.SubCategoryId = command.SubCategoryId;
        listing.CityId = command.CityId;

        _listingRepository.Update(listing);
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);
        if (listing == null) throw new Exception($"Listing with id {id} not found.");
        _listingRepository.Remove(listing);
        await _listingRepository.SaveChangesAsync(cancellationToken);
    }
}
