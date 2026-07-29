using Microsoft.Data.SqlClient;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Execution;
using TaskManagement.Infrastructure.Mappers;
using TaskManagement.Infrastructure.QueryManagement;
using TaskManagement.Infrastructure.QueryManagement.QueryNames;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDatabaseExecutor _databaseExecutor;
    private readonly ISqlQueryService _queryService;

    public TaskRepository(
        IDatabaseExecutor databaseExecutor,
        ISqlQueryService queryService)
    {
        _databaseExecutor = databaseExecutor;
        _queryService = queryService;
    }

    public async Task<IEnumerable<TaskItem>> GetByUserAsync(
        int userId)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.GetByUser);

        var parameters = new[]
        {
            new SqlParameter("@UserId", userId)
        };

        return await _databaseExecutor.QueryAsync(
            sql,
            parameters,
            TaskMapper.Map);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.GetById);

        var parameters = new[]
        {
            new SqlParameter("@Id", id)
        };

        return await _databaseExecutor.QuerySingleAsync(
            sql,
            parameters,
            TaskMapper.Map);
    }

    public async Task<int> CreateAsync(TaskItem task)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.Create);

        var parameters = new[]
        {
            new SqlParameter("@Title", task.Title),

            new SqlParameter(
                "@Description",
                (object?)task.Description ?? DBNull.Value),

            new SqlParameter(
                "@Status",
                (int)task.Status),

            new SqlParameter(
                "@Priority",
                (int)task.Priority),

            new SqlParameter(
                "@DueDate",
                task.DueDate),

            new SqlParameter(
                "@UserId",
                task.UserId)
        };

        return await _databaseExecutor.ExecuteScalarAsync<int>(
            sql,
            parameters);
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.Update);

        var parameters = new[]
        {
            new SqlParameter("@Id", task.Id),

            new SqlParameter("@Title", task.Title),

            new SqlParameter(
                "@Description",
                (object?)task.Description ?? DBNull.Value),

            new SqlParameter(
                "@Status",
                (int)task.Status),

            new SqlParameter(
                "@Priority",
                (int)task.Priority),

            new SqlParameter(
                "@DueDate",
                task.DueDate)
        };

        var rows = await _databaseExecutor.ExecuteAsync(
            sql,
            parameters);

        return rows > 0;
    }

    public async Task<bool> UpdateStatusAsync(
    int id,
    int status)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.UpdateStatus);

        var parameters = new[]
        {
        new SqlParameter("@Id", id),
        new SqlParameter("@Status", status)
    };

        var rows = await _databaseExecutor.ExecuteAsync(
            sql,
            parameters);

        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = _queryService.Get(
            TaskQueryNames.File,
            TaskQueryNames.Delete);

        var parameters = new[]
        {
            new SqlParameter("@Id", id)
        };

        var rows = await _databaseExecutor.ExecuteAsync(
            sql,
            parameters);

        return rows > 0;
    }
}