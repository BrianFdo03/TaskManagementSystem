using TaskManagement.Application.DTOs.Auth.Requests;
using TaskManagement.Application.DTOs.Auth.Responses;

namespace TaskManagement.Application.Contracts.Services
{
    internal interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
