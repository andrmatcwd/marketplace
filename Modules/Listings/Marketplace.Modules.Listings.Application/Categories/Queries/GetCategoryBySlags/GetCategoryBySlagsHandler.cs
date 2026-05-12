using System;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryBySlags;

public sealed class GetCategoryBySlagsHandler
    : IRequestHandler<GetCategoryBySlagsQuery, CategoryDto>
{
    private readonly ICategoryService _categoryRepository;

    public GetCategoryBySlagsHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<CategoryDto> Handle(GetCategoryBySlagsQuery request, CancellationToken cancellationToken)
    {
        return _categoryRepository.GetBySlagsAsync(
            request.CitySlag,
            request.CategorySlag,
            cancellationToken);
    }
}
