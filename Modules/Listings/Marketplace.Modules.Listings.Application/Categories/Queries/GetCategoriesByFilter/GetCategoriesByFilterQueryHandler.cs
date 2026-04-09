using Marketplace.Modules.Listings.Application.Common.Models;
using Marketplace.Modules.Listings.Application.Categories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetCategoriesByFilter;

public sealed class GetCategoriesByFilterQueryHandler : IRequestHandler<GetCategoriesByFilterQuery, PagedResult<CategoryDto>>
{
    public Task<PagedResult<CategoryDto>> Handle(GetCategoriesByFilterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new PagedResult<CategoryDto>());
    }
}
