namespace TaskManagement.Application.DTOs.Users.Requests;

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Role { get; set; }
}