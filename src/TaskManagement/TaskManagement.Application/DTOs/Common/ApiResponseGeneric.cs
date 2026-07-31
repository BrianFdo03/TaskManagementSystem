namespace TaskManagement.Application.DTOs.Common;

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}