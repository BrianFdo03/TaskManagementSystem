using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Execution;
using TaskManagement.Infrastructure.QueryManagement;

namespace TaskManagement.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDatabaseExecutor _databaseExecutor;
        private readonly ISqlQueryService _queryService;

        public AuthRepository(
            IDatabaseExecutor databaseExecutor,
            ISqlQueryService queryService)
        {
            _databaseExecutor = databaseExecutor;
            _queryService = queryService;
        }

        public Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
