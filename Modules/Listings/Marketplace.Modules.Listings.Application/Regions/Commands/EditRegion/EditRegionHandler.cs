using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.EditRegion;

public sealed class EditRegionHandler
    : IRequestHandler<EditRegionCommand, Unit>
{
    private readonly IRegionService _regionService;

    public EditRegionHandler(IRegionService regionService)
    {
        _regionService = regionService;
    }

    public async Task<Unit> Handle(EditRegionCommand request, CancellationToken cancellationToken)
    {
        await _regionService.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
