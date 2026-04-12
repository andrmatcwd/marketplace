using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface ICategoryRepository : IBaseRepository<Category, int>
{
    Task<(IReadOnlyCollection<Category> Items, int TotalCount)> GetByFilterAsync(
        CategoryFilter filter,
        CancellationToken cancellationToken = default);
}
