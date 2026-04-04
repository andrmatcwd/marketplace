using System;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;

public sealed class CreateListingHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingService listingService;

    public CreateListingHandler(IListingService listingService)
    {
        this.listingService = listingService;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
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

        return Guid.NewGuid(); // Placeholder until actual implementation is done
    }
}
