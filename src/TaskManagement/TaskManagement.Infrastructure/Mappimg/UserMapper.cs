using System.Data;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Mappers;

public static class UserMapper
{
    public static User Map(IDataReader reader)
    {
        return new User
        {
            Id = reader.GetRequiredInt32("Id"),
            Name = reader.GetRequiredString("Name"),
            Email = reader.GetRequiredString("Email"),
            PasswordHash = reader.GetRequiredString("PasswordHash"),
            Role = reader.GetRequiredEnum<UserRole>("Role"),
            CreatedDate = reader.GetRequiredDateTime("CreatedDate")
        };
    }
}