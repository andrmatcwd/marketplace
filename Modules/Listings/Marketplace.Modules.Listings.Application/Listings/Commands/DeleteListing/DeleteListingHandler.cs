using System;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;

public sealed class DeleteListingHandler
    : IRequestHandler<DeleteListingCommand, Unit>
{
    private readonly IListingService _listingService;

    public DeleteListingHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public async Task<Unit> Handle(DeleteListingCommand request, CancellationToken cancellationToken)
    {
        await _listingService.DeleteAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
