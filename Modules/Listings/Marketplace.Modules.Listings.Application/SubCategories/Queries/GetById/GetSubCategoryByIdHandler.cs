using Marketplace.Modules.Listings.Application.SubCategories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Queries.GetById;

public sealed class GetSubCategoryByIdHandler : IRequestHandler<GetSubCategoryByIdQuery, SubCategoryDto>
{
    public Task<SubCategoryDto> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new SubCategoryDto());
    }
}
