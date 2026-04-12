using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;

public sealed record EditCityCommand(
    int Id,
    int RegionId,
    string Name,
    string Slug) : IRequest<Unit>;
