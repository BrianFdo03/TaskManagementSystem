using System.Data;
using Microsoft.Data.SqlClient;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Execution;
using TaskManagement.Infrastructure.Mappers;
using TaskManagement.Infrastructure.QueryManagement;
using TaskManagement.Infrastructure.QueryManagement.QueryNames;

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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var sql = _queryService.Get(
                AuthQueryNames.File,
                AuthQueryNames.GetUserByEmail);

            var parameters = new[]
            {
            new SqlParameter("@Email", email)
        };

            return await _databaseExecutor.QuerySingleAsync(
                sql,
                parameters,
                UserMapper.Map);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var sql = _queryService.Get(
                AuthQueryNames.File,
                AuthQueryNames.CheckEmailExists);

            var parameters = new[]
            {
            new SqlParameter("@Email", email)
        };

            var count = await _databaseExecutor.ExecuteScalarAsync<int>(
                sql,
                parameters);

            return count > 0;
        }
    }
}