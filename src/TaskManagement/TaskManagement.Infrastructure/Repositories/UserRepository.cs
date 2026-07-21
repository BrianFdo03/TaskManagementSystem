using Microsoft.Data.SqlClient;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Execution;
using TaskManagement.Infrastructure.Mappers;
using TaskManagement.Infrastructure.QueryManagement;
using TaskManagement.Infrastructure.QueryManagement.QueryNames;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDatabaseExecutor _databaseExecutor;
    private readonly ISqlQueryService _queryService;

    public UserRepository(
        IDatabaseExecutor databaseExecutor,
        ISqlQueryService queryService)
    {
        _databaseExecutor = databaseExecutor;
        _queryService = queryService;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = _queryService.Get(
            UserQueryNames.File,
            UserQueryNames.GetAll);

        var users = await _databaseExecutor.QueryAsync(
            sql,
            null,
            MapUser);

        return users;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var sql = _queryService.Get(
            UserQueryNames.File,
            UserQueryNames.GetById);

        var parameters = new[]
        {
            Parameter("@Id", id)
        };

        return await _databaseExecutor.QuerySingleAsync(
            sql,
            parameters,
            UserMapper.Map);
    }

    //public Task<User?> GetByEmailAsync(string email)
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<int> CreateAsync(User user)
    {
        var sql = _queryService.Get(
            UserQueryNames.File,
            UserQueryNames.Create);

        var parameters = new[]
        {
        Parameter("@Name", user.Name),
        Parameter("@Email", user.Email),
        Parameter("@PasswordHash", user.PasswordHash),
        Parameter("@Role", (int)user.Role)
    };

        return await _databaseExecutor.ExecuteScalarAsync<int>(
            sql,
            parameters)
            ?? throw new InvalidOperationException(
                "Failed to create user.");
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = _queryService.Get(
            UserQueryNames.File,
            UserQueryNames.Delete);

        var parameters = new[]
        {
        Parameter("@Id", id)
    };

        var rows = await _databaseExecutor.ExecuteAsync(
            sql,
            parameters);

        return rows > 0;
    }

    //public Task<bool> EmailExistsAsync(string email)
    //{
    //    throw new NotImplementedException();
    //}

    //private static User MapUser(IDataReader reader)
    //{

    //    return new User
    //    {
    //        Id = reader.GetRequiredInt32("Id"),

    //        Name = reader.GetRequiredString("Name"),

    //        Email = reader.GetRequiredString("Email"),

    //        PasswordHash =
    //        reader.GetRequiredString("PasswordHash"),

    //        Role =
    //        reader.GetRequiredEnum<UserRole>("Role"),

    //        CreatedDate =
    //        reader.GetRequiredDateTime("CreatedDate")
    //    };
    //}


    private static SqlParameter Parameter(string name, object? value)
    {
        return new SqlParameter(name, value ?? DBNull.Value);
    }
}