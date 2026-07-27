using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Contracts.Security;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Auth.Requests;
using TaskManagement.Application.DTOs.Auth.Responses;

namespace TaskManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IAuthRepository authRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _authRepository = authRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _authRepository.GetUserByEmailAsync(
            request.Email);

        if (user == null)
            return null;

        var passwordValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordValid)
            return null;

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}