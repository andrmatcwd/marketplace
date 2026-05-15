using Marketplace.Modules.Listings.Application.Catalog.Dtos;
using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetPublicVacanciesQuery(VacancyListingFilter Filter)
    : IRequest<IReadOnlyList<PublicVacancyDto>>;

internal sealed class GetPublicVacanciesHandler(ICatalogDataService data)
    : IRequestHandler<GetPublicVacanciesQuery, IReadOnlyList<PublicVacancyDto>>
{
    public Task<IReadOnlyList<PublicVacancyDto>> Handle(
        GetPublicVacanciesQuery request, CancellationToken cancellationToken)
        => data.GetPublicVacanciesAsync(request.Filter, cancellationToken);
}
