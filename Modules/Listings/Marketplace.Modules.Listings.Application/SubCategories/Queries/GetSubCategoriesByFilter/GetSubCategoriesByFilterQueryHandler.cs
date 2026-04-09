using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetSubCategoriesByFilter;

public sealed class GetSubCategoriesByFilterQueryHandler : IRequestHandler<GetSubCategoriesByFilterQuery, PagedResult<SubCategoryDto>>
{
    public Task<PagedResult<SubCategoryDto>> Handle(GetSubCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<SubCategoryDto>());
    }
}
