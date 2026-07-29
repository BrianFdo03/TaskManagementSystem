namespace TaskManagement.Application.DTOs.Auth.Responses;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
}