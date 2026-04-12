using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.DeleteCity;

public sealed record DeleteCityCommand(int Id)
    : IRequest<Unit>;
