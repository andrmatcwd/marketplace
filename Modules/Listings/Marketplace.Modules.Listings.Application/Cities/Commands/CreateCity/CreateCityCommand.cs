using MediatR;

namespace Marketplace.Modules.Listings.Application.Cities.Commands.CreateCity;

public sealed record CreateCityCommand(
    string Name,
    string? Slug,
    string? Description,
    bool IsPublished,
    int SortOrder
) : IRequest<Unit>;
