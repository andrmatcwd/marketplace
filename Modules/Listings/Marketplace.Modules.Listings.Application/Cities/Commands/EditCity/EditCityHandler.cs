using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;

public sealed class EditCityHandler
    : IRequestHandler<EditCityCommand, Unit>
{
    private readonly ICityService _cityService;

    public EditCityHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Unit> Handle(EditCityCommand request, CancellationToken cancellationToken)
    {
        await _cityService.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
