using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Domain.Enums;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.UpdateContactRequestStatus;

public sealed class UpdateContactRequestStatusHandler : IRequestHandler<UpdateContactRequestStatusCommand, bool>
{
    private readonly IContactRequestRepository _repository;

    public UpdateContactRequestStatusHandler(IContactRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateContactRequestStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        entity.Status = request.Status;
        entity.AdminNotes = request.AdminNotes;

        if (request.Status is ContactRequestStatus.Processed or ContactRequestStatus.Rejected)
            entity.ProcessedAtUtc ??= DateTime.UtcNow;

        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
