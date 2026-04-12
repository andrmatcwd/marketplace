using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.EditListing;

public sealed record EditListingCommand(
    int Id,
    string Title,
    string Description,
    decimal Price,
    string SellerId,
    bool IsService
) : IRequest<Unit>;
