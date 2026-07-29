namespace TaskManagement.Application.DTOs.Tasks.Requests;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Status { get; set; }

    public int Priority { get; set; }

    public DateTime DueDate { get; set; }
}