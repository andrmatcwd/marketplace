using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.CreateListing;

public sealed record CreateListingCommand(
    string Title,
    string Description,
    decimal Price,
    string SellerId,
    bool IsService
) : IRequest<Guid>;