using Marketplace.Modules.Listings.Application.Services;
using MediatR;

namespace Marketplace.Modules.Listings.Application.Vacancies.Commands;

public sealed record DeleteVacancyCommand(int Id) : IRequest<Unit>;

internal sealed class DeleteVacancyHandler(IListingVacancyService service)
    : IRequestHandler<DeleteVacancyCommand, Unit>
{
    public async Task<Unit> Handle(DeleteVacancyCommand request, CancellationToken cancellationToken)
    {
        await service.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
