using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

    public TaskPriority Priority { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }

    // Foreign Key
    public int UserId { get; set; }

    // Navigation reference (logical only, not EF required)
    public User? User { get; set; }
}