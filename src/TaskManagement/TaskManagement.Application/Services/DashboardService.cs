using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Contracts.Services;
using TaskManagement.Application.DTOs.Dashboard.Responses;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Repositories;

namespace TaskManagement.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public DashboardService(
        ITaskRepository taskRepository,
        IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task<DashboardResponse> GetDashboardAsync(
        int userId,
        bool isAdmin)
    {
        var tasks = isAdmin
            ? await _taskRepository.GetAllAsync()
            : await _taskRepository.GetByUserAsync(userId);

        var taskList = tasks.ToList();

        var response = new DashboardResponse
        {
            TotalTasks = taskList.Count,

            PendingTasks = taskList.Count(task => task.Status == Domain.Enums.TaskStatus.Pending),

            InProgressTasks = taskList.Count(task => task.Status == Domain.Enums.TaskStatus.InProgress),

            CompletedTasks = taskList.Count(task => task.Status == Domain.Enums.TaskStatus.Completed),

            HighPriorityTasks = taskList.Count(task => task.Priority == TaskPriority.High)
        };

        if (isAdmin)
        {
            var users = await _userRepository.GetAllAsync();

            response.TotalUsers = users.Count();
        }

        return response;
    }
}