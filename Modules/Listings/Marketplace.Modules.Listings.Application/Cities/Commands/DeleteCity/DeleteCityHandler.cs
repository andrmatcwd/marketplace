using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.DeleteCity;

public sealed class DeleteCityHandler
    : IRequestHandler<DeleteCityCommand, Unit>
{
    private readonly ICityService _cityService;

    public DeleteCityHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Unit> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        await _cityService.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
