using Marketplace.Modules.Listings.Application.Listings.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Queries.GetById;

public sealed class GetListingByIdHandler : IRequestHandler<GetListingByIdQuery, ListingDto>
{
    private readonly IListingService listingService;

    public GetListingByIdHandler(IListingService listingService)
    {
        this.listingService = listingService;
    }

    public Task<ListingDto> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        // var listing = new Listing(
        //     Guid.NewGuid(),
        //     request.Title,
        //     request.Description,
        //     request.Price,
        //     request.SellerId,
        //     request.IsService ? ListingType.Service : ListingType.Product);

        // _dbContext.Listings.Add(listing);
        // await _dbContext.SaveChangesAsync(cancellationToken);

        // return listing.Id;

        return Task.FromResult(new ListingDto());
    }
}

