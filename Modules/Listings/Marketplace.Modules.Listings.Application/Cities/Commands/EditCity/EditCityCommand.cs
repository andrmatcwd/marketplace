using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.EditCity;

public sealed record EditCityCommand(
    int Id,
    string Name,
    string? Slug,
    string? Description,
    bool IsPublished,
    int SortOrder
) : IRequest<Unit>;
