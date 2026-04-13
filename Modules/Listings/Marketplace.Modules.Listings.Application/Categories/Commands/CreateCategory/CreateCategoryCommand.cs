using MediatR;

namespace Marketplace.Modules.Listings.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(
    int CityId,
    string Name,
    string? Description,
    string? Icon
) : IRequest<Unit>;
