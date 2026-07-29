using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Contracts.Security;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}