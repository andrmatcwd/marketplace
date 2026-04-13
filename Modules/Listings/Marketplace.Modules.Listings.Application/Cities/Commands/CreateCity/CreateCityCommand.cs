using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;

public sealed record CreateCityCommand(
    int RegionId,
    string Name
) : IRequest<Unit>;
