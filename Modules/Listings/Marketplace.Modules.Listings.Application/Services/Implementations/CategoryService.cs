using AutoMapper;
using Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;
using Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Categories.Filters;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category == null)
        {
            throw new Exception($"Category with id {id} not found.");
        }

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<PagedResult<CategoryDto>> GetByFilterAsync(CategoryFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _categoryRepository.GetByFilterAsync(filter, cancellationToken);

        return new PagedResult<CategoryDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<CategoryDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = command.Name
        };

        await _categoryRepository.AddAsync(category, cancellationToken);

        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            throw new Exception($"Category with id {command.Id} not found.");
        }

        category.Name = command.Name;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

        if (category == null)
        {
            throw new Exception($"Category with id {id} not found.");
        }

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChangesAsync(cancellationToken);
    }
}
