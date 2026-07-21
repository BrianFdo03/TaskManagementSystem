using Microsoft.Data.SqlClient;
using System.Data;
using TaskManagement.Infrastructure.Database;

namespace TaskManagement.Infrastructure.Execution;

public class DatabaseExecutor : IDatabaseExecutor
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseExecutor(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    #region QueryAsync

    public async Task<List<T>> QueryAsync<T>(
    string sql,
    IEnumerable<SqlParameter>? parameters,
    Func<IDataReader, T> mapper,
    DbExecutionContext? context = null)
    {
        var results = new List<T>();

        var (connection, shouldDispose) = await GetConnectionAsync(context);

        try
        {
            await using var command = CreateCommand(connection, sql, parameters, context);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                results.Add(mapper(reader));
            }

            return results;
        }
        finally
        {
            if (shouldDispose)
            {
                await connection.DisposeAsync();
            }
        }
    }

    #endregion

    #region QuerySingleAsync

    public async Task<T?> QuerySingleAsync<T>(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        Func<IDataReader, T> mapper,
        DbExecutionContext? context = null)
    {
        var result = await QueryAsync(sql, parameters, mapper, context);
        return result.FirstOrDefault();
    }

    #endregion

    #region ExecuteAsync

    public async Task<int> ExecuteAsync(
    string sql,
    IEnumerable<SqlParameter>? parameters,
    DbExecutionContext? context = null)
    {
        var (connection, shouldDispose) = await GetConnectionAsync(context);

        try
        {
            await using var command = CreateCommand(connection, sql, parameters, context);

            return await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (shouldDispose)
            {
                await connection.DisposeAsync();
            }
        }
    }

    #endregion

    #region ExecuteScalarAsync

    public async Task<T?> ExecuteScalarAsync<T>(
    string sql,
    IEnumerable<SqlParameter>? parameters,
    DbExecutionContext? context = null)
    {
        var (connection, shouldDispose) = await GetConnectionAsync(context);

        try
        {
            await using var command = CreateCommand(connection, sql, parameters, context);

            object? result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                return default;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }
        finally
        {
            if (shouldDispose)
            {
                await connection.DisposeAsync();
            }
        }
    }

    #endregion

    #region Helpers

    private async Task<(SqlConnection connection, bool shouldDispose)> GetConnectionAsync(DbExecutionContext? context)
    {
        // If transaction context exists → reuse connection
        if (context?.Connection != null)
        {
            return (context.Connection, false);
        }

        var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        return (connection, true);
    }

    private SqlCommand CreateCommand(
        SqlConnection connection,
        string sql,
        IEnumerable<SqlParameter>? parameters,
        DbExecutionContext? context)
    {
        var command = connection.CreateCommand();

        command.CommandText = sql;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 30;

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters.ToArray());
        }

        if (context?.Transaction != null)
        {
            command.Transaction = context.Transaction;
        }

        return command;
    }

    #endregion
}