using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;

public sealed class CreateListingHandler
    : IRequestHandler<CreateListingCommand, Unit>
{
    private readonly IListingService _listingService;

    public CreateListingHandler(IListingService listingService)
    {
        _listingService = listingService;
    }

    public async Task<Unit> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        await _listingService.AddAsync(request, cancellationToken);

        return Unit.Value;
    }
}
