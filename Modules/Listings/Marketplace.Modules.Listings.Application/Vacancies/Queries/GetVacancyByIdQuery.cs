using Marketplace.Modules.Listings.Application.Services;
using Marketplace.Modules.Listings.Application.Vacancies.Dtos;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Vacancies.Queries;

public sealed record GetVacancyByIdQuery(int Id) : IRequest<VacancyDto?>;

internal sealed class GetVacancyByIdHandler(IListingVacancyService service)
    : IRequestHandler<GetVacancyByIdQuery, VacancyDto?>
{
    public async Task<VacancyDto?> Handle(GetVacancyByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.Id, cancellationToken);
    }
}
