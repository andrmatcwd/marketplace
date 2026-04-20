using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryById;

public sealed class GetSubCategoryByIdHandler : IRequestHandler<GetSubCategoryByIdQuery, SubCategoryDto>
{
    private readonly ISubCategoryService _subCategoryService;

    public GetSubCategoryByIdHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }
    
    public Task<SubCategoryDto> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return _subCategoryService.GetByIdAsync(request.Id, cancellationToken);
    }
}
