using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Regions.Commands.DeleteRegion;

public sealed class DeleteRegionHandler
    : IRequestHandler<DeleteRegionCommand, Unit>
{
    private readonly IRegionService _regionService;

    public DeleteRegionHandler(IRegionService regionService)
    {
        _regionService = regionService;
    }
    
    public async Task<Unit> Handle(DeleteRegionCommand request, CancellationToken cancellationToken)
    {
        await _regionService.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
