using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;

public sealed record CreateRegionCommand(
    string Name,
    string Slug) : IRequest<Unit>;
