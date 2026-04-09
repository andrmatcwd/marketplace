using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.DeleteLocation;

public sealed record DeleteLocationCommand(int Id) : IRequest<int>;
