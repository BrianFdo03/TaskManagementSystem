using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<IEnumerable<TaskItem>> GetByUserAsync(int userId);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<int> CreateAsync(TaskItem task);
    Task<bool> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStatusAsync(int id, int status);
}