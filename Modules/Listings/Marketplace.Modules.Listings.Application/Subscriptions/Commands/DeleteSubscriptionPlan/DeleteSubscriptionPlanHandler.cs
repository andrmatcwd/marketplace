using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.DeleteSubscriptionPlan;

public sealed class DeleteSubscriptionPlanHandler : IRequestHandler<DeleteSubscriptionPlanCommand, Unit>
{
    private readonly ISubscriptionPlanService _service;

    public DeleteSubscriptionPlanHandler(ISubscriptionPlanService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(DeleteSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
