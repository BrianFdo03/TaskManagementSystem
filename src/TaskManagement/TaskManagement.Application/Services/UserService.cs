using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Users.Requests;
using TaskManagement.Application.DTOs.Users.Responses;
using TaskManagement.Application.Mapping;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
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
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,

            // Temporary
            PasswordHash = request.Password,

            Role = (UserRole)request.Role
        };

        return await _repository.CreateAsync(user);

    }

    public async Task<bool> UpdateAsync(
    int id,
    UpdateUserRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);

        if (existing == null)
            return false;

        existing.Name = request.Name;
        existing.Email = request.Email;
        existing.Role = (UserRole)request.Role;

        return await _repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}