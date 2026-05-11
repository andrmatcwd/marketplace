using AutoMapper;
using Marketplace.Modules.Users.Application.Common.Models;
using Marketplace.Modules.Users.Application.Repositories;
using Marketplace.Modules.Users.Application.Users.Commands.CreateUser;
using Marketplace.Modules.Users.Application.Users.Commands.EditUser;
using Marketplace.Modules.Users.Application.Users.Dtos;
using Marketplace.Modules.Users.Application.Users.Filters;
using Marketplace.Modules.Users.Domain.Entities;

namespace Marketplace.Modules.Users.Application.Services.Implementations;

public class AppUserService : IAppUserService
{
    private readonly IAppUserRepository _repository;
    private readonly IMapper _mapper;

    public AppUserService(IAppUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdentityUserIdAsync(identityUserId, cancellationToken);
        return user is null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<PagedResult<UserDto>> GetByFilterAsync(UserFilter filter, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetByFilterAsync(filter, cancellationToken);
        return new PagedResult<UserDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<UserDto>>(items),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task AddAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = new AppUser
        {
            IdentityUserId = command.IdentityUserId,
            DisplayName = command.DisplayName,
            Email = command.Email,
            Phone = command.Phone,
            AvatarUrl = command.AvatarUrl,
            Bio = command.Bio,
            Status = command.Status
        };

        await _repository.AddAsync(user, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task EditAsync(EditUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new Exception($"User with id {command.Id} not found.");

        user.DisplayName = command.DisplayName;
        user.Email = command.Email;
        user.Phone = command.Phone;
        user.AvatarUrl = command.AvatarUrl;
        user.Bio = command.Bio;
        user.Status = command.Status;
        user.UpdatedAtUtc = DateTime.UtcNow;

        _repository.Update(user);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken)
            ?? throw new Exception($"User with id {id} not found.");

        _repository.Remove(user);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
