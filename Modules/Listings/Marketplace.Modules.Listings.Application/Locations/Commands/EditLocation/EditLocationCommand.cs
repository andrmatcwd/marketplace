using MediatR;

namespace Marketplace.Modules.Listings.Application.Locations.Commands.EditLocation;

public sealed record EditLocationCommand(int Id, int RegionId, string Name, string Slug, Marketplace.Modules.Listings.Domain.Enums.LocationType Type) : IRequest<int>;
