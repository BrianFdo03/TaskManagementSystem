using TaskManagement.Application.DTOs.Users.Responses;
using TaskManagement.Domain.Entities;

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
}