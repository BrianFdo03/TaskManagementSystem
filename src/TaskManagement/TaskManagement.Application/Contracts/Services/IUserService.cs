using TaskManagement.Application.DTOs.Users.Requests;
using TaskManagement.Application.DTOs.Users.Responses;

namespace TaskManagement.Application.Contracts.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task<UserResponse?> GetByIdAsync(int id);

    Task<int> CreateAsync(CreateUserRequest request);

    Task<bool> UpdateAsync(
        int id,
        UpdateUserRequest request);


    Task<bool> DeleteAsync(int id);
}