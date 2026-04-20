using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;

namespace Marketplace.Modules.Listings.Application.Services;

public interface ISubCategoryService
{
    Task<SubCategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<SubCategoryDto> GetBySlagsAsync(string citySlag, string categorySlag, string subCategorySlag, CancellationToken cancellationToken);

    Task<PagedResult<SubCategoryDto>> GetByFilterAsync(SubCategoryFilter filter, CancellationToken cancellationToken);

    Task AddAsync(CreateSubCategoryCommand command, CancellationToken cancellationToken);

    Task EditAsync(EditSubCategoryCommand command, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
