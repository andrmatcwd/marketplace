using Marketplace.Modules.Listings.Application.Categories.Dtos;
using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdHandler
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryService _categoryRepository;

    public GetCategoryByIdHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
