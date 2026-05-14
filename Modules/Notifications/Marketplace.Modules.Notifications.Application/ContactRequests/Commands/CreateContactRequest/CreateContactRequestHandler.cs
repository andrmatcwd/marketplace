using Marketplace.Modules.Notifications.Application.Repositories;
using Marketplace.Modules.Notifications.Domain.Entities;
using MediatR;

namespace Marketplace.Modules.Notifications.Application.ContactRequests.Commands.CreateContactRequest;

public sealed class CreateContactRequestHandler : IRequestHandler<CreateContactRequestCommand, int>
{
    private readonly IContactRequestRepository _repository;

    public CreateContactRequestHandler(IContactRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(CreateContactRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = new ContactRequest
        {
            Type = request.Type,
            CustomerName = request.CustomerName,
            CustomerPhone = request.CustomerPhone,
            CustomerEmail = request.CustomerEmail,
            CustomerCompany = request.CustomerCompany,
            Message = request.Message,
            ListingId = request.ListingId,
            ListingTitle = request.ListingTitle,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
