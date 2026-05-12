using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using MediatR;
using Marketplace.Modules.Listings.Application.Services;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;

public sealed class GetCategoriesByFilterHandler
    : IRequestHandler<GetCategoriesByFilterQuery, PagedResult<CategoryDto>>
{
    private readonly ICategoryService _categoryRepository;

    public GetCategoriesByFilterHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<PagedResult<CategoryDto>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        return _categoryRepository.GetByFilterAsync(request.Filter, cancellationToken);
    }
}
