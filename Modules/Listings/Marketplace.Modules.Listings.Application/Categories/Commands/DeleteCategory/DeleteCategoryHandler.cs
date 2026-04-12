using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryService _categoryRepository;

    public DeleteCategoryHandler(ICategoryService categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
