using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;

public sealed class EditListingHandler
    : IRequestHandler<EditListingCommand, Unit>
{
    private readonly IListingService _listingService;

    public EditListingHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public async Task<Unit> Handle(EditListingCommand request, CancellationToken cancellationToken)
    {
        await _listingService.EditAsync(request, cancellationToken);

        return Unit.Value;
    }
}

