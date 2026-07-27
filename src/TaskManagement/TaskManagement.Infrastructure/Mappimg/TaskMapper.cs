using System.Data;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Mappers;

public static class TaskMapper
{
    public static TaskItem Map(IDataReader reader)
    {
        return new TaskItem
        {
            Id = reader.GetRequiredInt32("Id"),

            Title = reader.GetRequiredString("Title"),

            Description = reader.GetNullableString("Description"),

            Status = reader.GetRequiredEnum<Domain.Enums.TaskStatus>("Status"),

            Priority = reader.GetRequiredEnum<TaskPriority>("Priority"),

            DueDate = reader.GetRequiredDateTime("DueDate"),

            CreatedDate = reader.GetRequiredDateTime("CreatedDate"),

            UpdatedDate = reader.GetNullableDateTime("UpdatedDate"),

            UserId = reader.GetRequiredInt32("UserId")
        };
    }
}