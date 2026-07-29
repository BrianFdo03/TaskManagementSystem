using TaskManagement.Application.DTOs.Users.Requests;
using TaskManagement.Application.DTOs.Users.Responses;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;


namespace TaskManagement.Application.Mapping;

public static class UserMapping
{
    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedDate = user.CreatedDate
        };
    }

    public static User ToEntity(CreateUserRequest request)
    {
        return new User
        {
            Name = request.Name,
            Email = request.Email,
            Role = (UserRole)request.Role
        };
    }

    public static void ApplyUpdate(
        User user,
        UpdateUserRequest request)
    {
        user.Name = request.Name;
        user.Email = request.Email;
        user.Role = (UserRole)request.Role;
    }
}