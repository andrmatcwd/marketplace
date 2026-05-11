using Marketplace.Modules.Users.Application.Services;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.EditUser;

public sealed class EditUserHandler : IRequestHandler<EditUserCommand, Unit>
{
    private readonly IAppUserService _service;

    public EditUserHandler(IAppUserService service) => _service = service;

    public async Task<Unit> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        await _service.EditAsync(request, cancellationToken);
        return Unit.Value;
    }
}
