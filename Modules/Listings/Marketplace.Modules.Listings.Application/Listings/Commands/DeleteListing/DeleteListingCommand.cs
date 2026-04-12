using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;

public sealed record DeleteListingCommand(int Id)
    : IRequest<Unit>;
