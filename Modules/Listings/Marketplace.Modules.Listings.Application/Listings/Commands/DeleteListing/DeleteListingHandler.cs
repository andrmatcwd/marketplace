using System;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;

public sealed class DeleteListingHandler : IRequestHandler<DeleteListingCommand, int>
{
    private readonly IListingService listingService;

    public DeleteListingHandler(IListingService listingService)
    {
        this.listingService = listingService;
    }

    public async Task<int> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
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

        return 0; // Placeholder until actual implementation is done
    }
}
