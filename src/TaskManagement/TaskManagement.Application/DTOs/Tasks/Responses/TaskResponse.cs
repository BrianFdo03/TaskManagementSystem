namespace TaskManagement.Application.DTOs.Tasks.Responses;

    public class TaskResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Priority { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }

