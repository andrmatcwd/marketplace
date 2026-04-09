using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
{
    public Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}
