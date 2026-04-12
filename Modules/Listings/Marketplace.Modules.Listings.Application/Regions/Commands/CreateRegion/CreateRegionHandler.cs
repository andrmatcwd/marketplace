using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.CreateRegion;

public sealed class CreateRegionHandler
    : IRequestHandler<CreateRegionCommand, Unit>
{
    private readonly IRegionService _regionService;

    public CreateRegionHandler(IRegionService regionService)
    {
        _regionService = regionService;
    }
    
    public async Task<Unit> Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        await _regionService.AddAsync(request, cancellationToken);
        return Unit.Value;
    }
}
