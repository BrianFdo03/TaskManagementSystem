using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Tasks.Requests;
using TaskManagement.Application.DTOs.Tasks.Responses;
using TaskManagement.Application.Mapping;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskResponse>> GetMyTasksAsync( // Tasks of own by emp
        int userId)
    {
        var tasks = await _taskRepository.GetByUserAsync(userId);

        return tasks.Select(TaskMapping.ToResponse);
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksByUserAsync( // Tasks of a user by admin
    int userId)
    {
        var tasks = await _taskRepository.GetByUserAsync(userId);

        return tasks.Select(TaskMapping.ToResponse);
    }

    public async Task<TaskResponse?> GetByIdAsync(
        int id,
        int userId,
        bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
            return null;

        if (!isAdmin && task.UserId != userId)
            return null;

        return TaskMapping.ToResponse(task);
    }

    public async Task<TaskResponse> CreateAsync(
    CreateTaskRequest request,
    int currentUserId,
    bool isAdmin)
    {
        var targetUserId = currentUserId;

        if (isAdmin && request.UserId.HasValue)
        {
            targetUserId = request.UserId.Value;
        }

        var task = TaskMapping.ToEntity(
            request,
            targetUserId);

        var id = await _taskRepository.CreateAsync(task);

        task.Id = id;

        return TaskMapping.ToResponse(task);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateTaskRequest request,
        int userId,
    bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
            return false;

        if (!isAdmin && task.UserId != userId)
            return false;

        TaskMapping.ApplyUpdate(
            task,
            request);

        return await _taskRepository.UpdateAsync(task);
    }

    public async Task<bool> UpdateStatusAsync(
        int id,
        int status,
        int userId,
    bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
            return false;

        if (!isAdmin && task.UserId != userId)
            return false;

        return await _taskRepository.UpdateStatusAsync(
            id,
            status);
    }

    public async Task<bool> DeleteAsync(
        int id,
        int userId,
    bool isAdmin)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        if (task == null)
            return false;

        if (!isAdmin && task.UserId != userId)
            return false;

        return await _taskRepository.DeleteAsync(id);
    }
}