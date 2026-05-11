using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Vacancies.Commands;

public sealed record CreateVacancyCommand(
    int ListingId,
    string Title,
    string? Description,
    string? EmploymentType,
    string? SalaryText,
    string? LocationText) : IRequest<Unit>;

internal sealed class CreateVacancyHandler(IListingVacancyService service)
    : IRequestHandler<CreateVacancyCommand, Unit>
{
    public async Task<Unit> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
    {
        await service.AddAsync(request, cancellationToken);
        return Unit.Value;
    }
}
