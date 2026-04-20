using System;
using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoryBySlags;

public class GetSubCategoryBySlagsHandler
    : IRequestHandler<GetSubCategoryBySlagsQuery, SubCategoryDto>
{
    private readonly ISubCategoryService _subCategoryService;

    public GetSubCategoryBySlagsHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }
    
    public Task<SubCategoryDto> Handle(GetSubCategoryBySlagsQuery request, CancellationToken cancellationToken)
    {
        return _subCategoryService.GetBySlagsAsync(
            request.CitySlag,
            request.CategorySlag,
            request.SubCategorySlag,
            cancellationToken);
    }
}
