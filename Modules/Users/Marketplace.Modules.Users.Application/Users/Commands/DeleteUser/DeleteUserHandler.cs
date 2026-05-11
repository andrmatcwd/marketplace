using Marketplace.Modules.Users.Application.Services;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IAppUserService _service;

    public DeleteUserHandler(IAppUserService service) => _service = service;

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
