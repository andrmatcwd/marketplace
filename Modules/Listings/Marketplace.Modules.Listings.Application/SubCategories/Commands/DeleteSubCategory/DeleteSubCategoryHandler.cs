using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.SubCategories.Commands.DeleteSubCategory;

public sealed class DeleteSubCategoryHandler : IRequestHandler<DeleteSubCategoryCommand, Unit>
{
    private readonly ISubCategoryService _subCategoryService;

    public DeleteSubCategoryHandler(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }
    
    public async Task<Unit> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
    {
        await _subCategoryService.DeleteAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
