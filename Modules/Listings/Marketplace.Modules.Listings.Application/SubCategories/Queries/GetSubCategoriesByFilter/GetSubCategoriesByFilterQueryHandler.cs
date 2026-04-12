using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;

public sealed class GetSubCategoriesByFilterQueryHandler : IRequestHandler<GetSubCategoriesByFilterQuery, PagedResult<SubCategoryDto>>
{
    private readonly ISubCategoryService _subCategoryService;

    public GetSubCategoriesByFilterQueryHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public Task<PagedResult<SubCategoryDto>> Handle(GetSubCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        return _subCategoryService.GetByFilterAsync(request.Filter, cancellationToken);
    }
}
