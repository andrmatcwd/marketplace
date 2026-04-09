using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.DeleteRegion;

public sealed record DeleteRegionCommand(int Id) : IRequest<int>;
