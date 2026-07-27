using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Contracts.Security;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Users.Requests;
using TaskManagement.Application.DTOs.Users.Responses;
using TaskManagement.Application.Mapping;

namespace TaskManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();

        return users.Select(UserMapping.ToResponse);
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return null;

        return UserMapping.ToResponse(user);
    }

    public async Task<int> CreateAsync(CreateUserRequest request)
    {
        var user = UserMapping.ToEntity(request);

        user.PasswordHash = _passwordHasher.Hash(request.Password);

        return await _repository.CreateAsync(user);

    }

    public async Task<bool> UpdateAsync(
    int id,
    UpdateUserRequest request)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
            return false;

        UserMapping.ApplyUpdate(user, request);

        return await _repository.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}