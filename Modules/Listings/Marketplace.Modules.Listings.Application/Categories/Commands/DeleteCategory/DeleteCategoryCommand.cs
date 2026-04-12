using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(int Id)
    : IRequest<Unit>;
