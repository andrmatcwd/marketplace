using Marketplace.Modules.Listings.Application.Categories.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Queries.GetById;

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CategoryDto());
    }
}
