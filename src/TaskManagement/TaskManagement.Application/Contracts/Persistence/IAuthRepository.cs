using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Contracts.Persistence
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);

    }
}
