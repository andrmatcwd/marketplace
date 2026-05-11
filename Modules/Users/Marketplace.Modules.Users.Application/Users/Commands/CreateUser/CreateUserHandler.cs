using Marketplace.Modules.Users.Application.Services;
using MediatR;

namespace Marketplace.Modules.Users.Application.Users.Commands.CreateUser;

public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, Unit>
{
    private readonly IAppUserService _service;

    public CreateUserHandler(IAppUserService service) => _service = service;

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _service.AddAsync(request, cancellationToken);
        return Unit.Value;
    }
}
