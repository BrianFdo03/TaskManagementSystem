namespace TaskManagement.Application.DTOs.Dashboard.Responses;

public class DashboardResponse
{
    public int TotalUsers { get; set; }

    public int TotalTasks { get; set; }

    public int PendingTasks { get; set; }

    public int InProgressTasks { get; set; }

    public int CompletedTasks { get; set; }

    public int HighPriorityTasks { get; set; }
}