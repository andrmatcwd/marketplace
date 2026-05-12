using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryHandler
    : IRequestHandler<CreateCategoryCommand, Unit>
{
    private readonly ICategoryService _categoryRepository;

    public CreateCategoryHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryRepository.AddAsync(request, cancellationToken);
        return Unit.Value;
    }
}
