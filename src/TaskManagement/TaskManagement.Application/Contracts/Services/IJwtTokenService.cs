using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Contracts.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}