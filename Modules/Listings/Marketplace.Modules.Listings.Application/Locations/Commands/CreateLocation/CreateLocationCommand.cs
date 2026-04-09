using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.CreateLocation;

public sealed record CreateLocationCommand(int RegionId, string Name, string Slug, Marketplace.Modules.Listings.Domain.Enums.LocationType Type) : IRequest<int>;
