using TaskManagement.Application.DTOs.Tasks.Requests;
using TaskManagement.Application.DTOs.Tasks.Responses;

namespace TaskManagement.Application.Contracts.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetMyTasksAsync( // Tasks of own by emp
        int userId);

    Task<IEnumerable<TaskResponse>> GetTasksByUserAsync(int userId); // Tasks of user by admin

    Task<TaskResponse?> GetByIdAsync(
        int id,
        int userId);

    Task<TaskResponse> CreateAsync(
        CreateTaskRequest request,
        int userId);

    Task<bool> UpdateAsync(
        int id,
        UpdateTaskRequest request,
        int userId);

    Task<bool> UpdateStatusAsync(
        int id,
        int status,
        int userId);

    Task<bool> DeleteAsync(
        int id,
        int userId);
}