using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.EditCategory;

public sealed record EditCategoryCommand(
    int Id,
    int CityId,
    string Name,
    string? Description,
    string? Icon
) : IRequest<Unit>;
