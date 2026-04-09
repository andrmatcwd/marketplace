using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.CreateSubCategory;

public sealed class CreateSubCategoryHandler : IRequestHandler<CreateSubCategoryCommand, int>
{
    public Task<int> Handle(CreateSubCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
