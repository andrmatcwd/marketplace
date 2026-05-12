using Marketplace.Modules.Listings.Application.Subscriptions.Dtos;

namespace Marketplace.Modules.Listings.Application.Subscriptions.Services;

public interface ISubscriptionPlanService
{
    Task<IReadOnlyList<SubscriptionPlanDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<SubscriptionPlanDto>> GetActiveAsync(CancellationToken cancellationToken);
    Task<SubscriptionPlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateAsync(string name, string? description, int subscriptionType, decimal priceUah, int durationDays, bool isActive, int displayOrder, CancellationToken cancellationToken);
    Task UpdateAsync(int id, string name, string? description, int subscriptionType, decimal priceUah, int durationDays, bool isActive, int displayOrder, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
