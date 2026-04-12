using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Repositories;

public interface ISubCategoryRepository : IBaseRepository<SubCategory, int>
{
    Task<(IReadOnlyCollection<SubCategory> Items, int TotalCount)> GetByFilterAsync(
        SubCategoryFilter filter,
        CancellationToken cancellationToken = default);
}
