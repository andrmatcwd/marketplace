using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;
using Marketplace.Modules.Listings.Domain.Entities;
using Marketplace.Modules.Listings.Domain.Enums.Subscription;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Services.Implementations;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _repository;

    public SubscriptionPlanService(ISubscriptionPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<SubscriptionPlanDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var plans = await _repository.GetAllAsync(cancellationToken);
        return plans.OrderBy(x => x.DisplayOrder).Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<SubscriptionPlanDto>> GetActiveAsync(CancellationToken cancellationToken)
    {
        var plans = await _repository.GetActiveAsync(cancellationToken);
        return plans.OrderBy(x => x.DisplayOrder).Select(ToDto).ToList();
    }

    public async Task<SubscriptionPlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(id, cancellationToken);
        return plan is null ? null : ToDto(plan);
    }

    public async Task<int> CreateAsync(string name, string? description, int subscriptionType, decimal priceUah, int durationDays, bool isActive, int displayOrder, CancellationToken cancellationToken)
    {
        var plan = new SubscriptionPlan
        {
            Name = name,
            Description = description,
            SubscriptionType = (SubscriptionType)subscriptionType,
            PriceUah = priceUah,
            DurationDays = durationDays,
            IsActive = isActive,
            DisplayOrder = displayOrder,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(plan, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return plan.Id;
    }

    public async Task UpdateAsync(int id, string name, string? description, int subscriptionType, decimal priceUah, int durationDays, bool isActive, int displayOrder, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception($"SubscriptionPlan {id} not found.");

        plan.Name = name;
        plan.Description = description;
        plan.SubscriptionType = (SubscriptionType)subscriptionType;
        plan.PriceUah = priceUah;
        plan.DurationDays = durationDays;
        plan.IsActive = isActive;
        plan.DisplayOrder = displayOrder;

        _repository.Update(plan);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception($"SubscriptionPlan {id} not found.");
        _repository.Remove(plan);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static SubscriptionPlanDto ToDto(SubscriptionPlan p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        SubscriptionType = p.SubscriptionType,
        PriceUah = p.PriceUah,
        DurationDays = p.DurationDays,
        IsActive = p.IsActive,
        DisplayOrder = p.DisplayOrder,
        CreatedAtUtc = p.CreatedAtUtc
    };
}
