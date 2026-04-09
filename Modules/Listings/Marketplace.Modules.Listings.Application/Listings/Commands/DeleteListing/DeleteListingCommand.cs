using MediatR;

namespace Marketplace.Modules.Listings.Application.Listings.Commands.DeleteListing;

public sealed record DeleteListingCommand(Guid Id) : IRequest<Guid>;
