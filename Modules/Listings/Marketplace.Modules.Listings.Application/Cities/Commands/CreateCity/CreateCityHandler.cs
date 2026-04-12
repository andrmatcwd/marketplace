using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;

public sealed class CreateCityHandler
    : IRequestHandler<CreateCityCommand, Unit>
{
    private readonly ICityService _cityService;

    public CreateCityHandler(ICityService cityService)
    {
        _cityService = cityService;
    }

    public async Task<Unit> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        await _cityService.AddAsync(request, cancellationToken);

        return Unit.Value;
    }
}
