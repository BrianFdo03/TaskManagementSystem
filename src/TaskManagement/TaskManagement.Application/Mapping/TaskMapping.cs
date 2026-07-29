using TaskManagement.Application.DTOs.Tasks.Requests;
using TaskManagement.Application.DTOs.Tasks.Responses;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Mapping;

public static class TaskMapping
{
    public static TaskResponse ToResponse(TaskItem task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            DueDate = task.DueDate,
            CreatedDate = task.CreatedDate,
            UpdatedDate = task.UpdatedDate
        };
    }

    public static TaskItem ToEntity(
        CreateTaskRequest request,
        int userId)
    {
        return new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = (Domain.Enums.TaskStatus)request.Status,
            Priority = (TaskPriority)request.Priority,
            DueDate = request.DueDate,
            UserId = userId
        };
    }

    public static void ApplyUpdate(
        TaskItem task,
        UpdateTaskRequest request)
    {
        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = (Domain.Enums.TaskStatus)request.Status;
        task.Priority = (TaskPriority)request.Priority;
        task.DueDate = request.DueDate;
    }
}