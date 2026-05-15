using Marketplace.Modules.Listings.Application.Catalog.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Catalog.Queries;

public sealed record CountPublicVacanciesQuery(VacancyListingFilter Filter)
    : IRequest<int>;

internal sealed class CountPublicVacanciesHandler(ICatalogDataService data)
    : IRequestHandler<CountPublicVacanciesQuery, int>
{
    public Task<int> Handle(CountPublicVacanciesQuery request, CancellationToken cancellationToken)
        => data.CountPublicVacanciesAsync(request.Filter, cancellationToken);
}
