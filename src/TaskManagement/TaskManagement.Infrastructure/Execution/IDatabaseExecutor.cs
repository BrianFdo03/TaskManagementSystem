using Microsoft.Data.SqlClient;
using System.Data;

namespace TaskManagement.Infrastructure.Execution;

public interface IDatabaseExecutor
{
    Task<List<T>> QueryAsync<T>(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        Func<IDataReader, T> mapper,
        DbExecutionContext? context = null);

    Task<T?> QuerySingleAsync<T>(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        Func<IDataReader, T> mapper,
        DbExecutionContext? context = null);

    Task<int> ExecuteAsync(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        DbExecutionContext? context = null);

    Task<T?> ExecuteScalarAsync<T>(
        string sql,
        IEnumerable<SqlParameter>? parameters,
        DbExecutionContext? context = null);
}