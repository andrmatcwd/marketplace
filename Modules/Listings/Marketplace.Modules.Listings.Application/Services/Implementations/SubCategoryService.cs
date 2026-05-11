using AutoMapper;
using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Repositories;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Commands.EditSubCategory;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using Marketplace.Modules.Listings.Application.SubCategories.Filters;
using Marketplace.Modules.Listings.Domain.Entities;

namespace Marketplace.Modules.Listings.Application.Services.Implementations;

public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ISlugService _slugService;
    private readonly IMapper _mapper;

    public SubCategoryService(ISubCategoryRepository subCategoryRepository, ISlugService slugService, IMapper mapper)
    {
        _subCategoryRepository = subCategoryRepository;
        _slugService = slugService;
        _mapper = mapper;
    }

    public async Task<SubCategoryDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (subCategory == null) throw new Exception($"SubCategory with id {id} not found.");
        return _mapper.Map<SubCategoryDto>(subCategory);
    }

    public async Task<PagedResult<SubCategoryDto>> GetByFilterAsync(SubCategoryFilter filter, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _subCategoryRepository.GetByFilterAsync(filter, cancellationToken);
        return new PagedResult<SubCategoryDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<SubCategoryDto>>(items),
            TotalCount = totalCount
        };
    }

    public async Task AddAsync(CreateSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var subCategory = new SubCategory
        {
            CategoryId = command.CategoryId,
            Name = command.Name,
            Slug = string.IsNullOrWhiteSpace(command.Slug)
                ? _slugService.Generate(command.Name)
                : command.Slug.Trim().ToLowerInvariant(),
            Description = command.Description,
            Icon = command.Icon,
            IsPublished = command.IsPublished,
            SortOrder = command.SortOrder
        };

        await _subCategoryRepository.AddAsync(subCategory, cancellationToken);
        await _subCategoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditSubCategoryCommand command, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(command.Id, cancellationToken);
        if (subCategory == null) throw new Exception($"SubCategory with id {command.Id} not found.");

        subCategory.CategoryId = command.CategoryId;
        subCategory.Name = command.Name;
        subCategory.Slug = string.IsNullOrWhiteSpace(command.Slug)
            ? _slugService.Generate(command.Name)
            : command.Slug.Trim().ToLowerInvariant();
        subCategory.Description = command.Description;
        subCategory.Icon = command.Icon;
        subCategory.IsPublished = command.IsPublished;
        subCategory.SortOrder = command.SortOrder;

        _subCategoryRepository.Update(subCategory);
        await _subCategoryRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var subCategory = await _subCategoryRepository.GetByIdAsync(id, cancellationToken);
        if (subCategory == null) throw new Exception($"SubCategory with id {id} not found.");
        _subCategoryRepository.Remove(subCategory);
        await _subCategoryRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<SubCategoryDto> GetBySlagsAsync(string citySlag, string categorySlag, string subCategorySlag, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
