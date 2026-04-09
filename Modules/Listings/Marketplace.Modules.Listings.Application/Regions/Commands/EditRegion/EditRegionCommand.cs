using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;

public sealed record EditRegionCommand(int Id, string Name, string Slug) : IRequest<int>;
