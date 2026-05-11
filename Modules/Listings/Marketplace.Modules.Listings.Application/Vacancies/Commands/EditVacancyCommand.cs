using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Vacancies.Commands;

public sealed record EditVacancyCommand(
    int Id,
    string Title,
    string? Description,
    string? EmploymentType,
    string? SalaryText,
    string? LocationText) : IRequest<Unit>;

internal sealed class EditVacancyHandler(IListingVacancyService service)
    : IRequestHandler<EditVacancyCommand, Unit>
{
    public async Task<Unit> Handle(EditVacancyCommand request, CancellationToken cancellationToken)
    {
        await service.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
