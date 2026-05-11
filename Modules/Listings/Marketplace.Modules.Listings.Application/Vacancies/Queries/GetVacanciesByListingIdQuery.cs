using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.Vacancies.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Vacancies.Queries;

public sealed record GetVacanciesByListingIdQuery(int ListingId) : IRequest<IReadOnlyList<VacancyDto>>;

internal sealed class GetVacanciesByListingIdHandler(IListingVacancyService service)
    : IRequestHandler<GetVacanciesByListingIdQuery, IReadOnlyList<VacancyDto>>
{
    public async Task<IReadOnlyList<VacancyDto>> Handle(GetVacanciesByListingIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByListingIdAsync(request.ListingId, cancellationToken);
    }
}
