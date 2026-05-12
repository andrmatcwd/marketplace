using Marketplace.Modules.Listings.Application.Subscriptions.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Commands.EditSubscriptionPlan;

public sealed class EditSubscriptionPlanHandler : IRequestHandler<EditSubscriptionPlanCommand, Unit>
{
    private readonly ISubscriptionPlanService _service;

    public EditSubscriptionPlanHandler(ISubscriptionPlanService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(EditSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        await _service.UpdateAsync(
            request.Id,
            request.Name,
            request.Description,
            request.SubscriptionType,
            request.PriceUah,
            request.DurationDays,
            request.IsActive,
            request.DisplayOrder,
            cancellationToken);

        return Unit.Value;
    }
}
