using Marketplace.Modules.Notifications.Application.Repositories;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.DeleteContactRequest;

public sealed class DeleteContactRequestHandler : IRequestHandler<DeleteContactRequestCommand, bool>
{
    private readonly IContactRequestRepository _repository;

    public DeleteContactRequestHandler(IContactRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteContactRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return false;

        _repository.Remove(entity);
        await _repository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
