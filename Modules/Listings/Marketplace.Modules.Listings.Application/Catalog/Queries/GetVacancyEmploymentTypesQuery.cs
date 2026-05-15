using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record GetVacancyEmploymentTypesQuery : IRequest<IReadOnlyList<string>>;

internal sealed class GetVacancyEmploymentTypesHandler(ICatalogDataService data)
    : IRequestHandler<GetVacancyEmploymentTypesQuery, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(
        GetVacancyEmploymentTypesQuery request, CancellationToken cancellationToken)
        => data.GetDistinctVacancyEmploymentTypesAsync(cancellationToken);
}
