using Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Common.Models;

namespace Marketplace.Modules.Listings.Application.Services;

public interface ICategoryService
{
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<PagedResult<CategoryDto>> GetByFilterAsync(CategoryFilter filter, CancellationToken cancellationToken);

    Task AddAsync(CreateCategoryCommand command, CancellationToken cancellationToken);

    Task EditAsync(EditCategoryCommand command, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
