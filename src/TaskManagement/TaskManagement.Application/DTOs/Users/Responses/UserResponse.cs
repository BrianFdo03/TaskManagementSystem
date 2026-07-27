namespace TaskManagement.Application.DTOs.Users.Responses;

public class UserResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }
}