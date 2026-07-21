using System.Data;

namespace TaskManagement.Infrastructure.Data;

public static class DataReaderExtensions
{
    public static int GetRequiredInt32(
        this IDataReader reader,
        string columnName)
    {
        return reader.GetInt32(
            reader.GetOrdinal(columnName));
    }


    public static string GetRequiredString(
        this IDataReader reader,
        string columnName)
    {
        return reader.GetString(
            reader.GetOrdinal(columnName));
    }


    public static DateTime GetRequiredDateTime(
        this IDataReader reader,
        string columnName)
    {
        return reader.GetDateTime(
            reader.GetOrdinal(columnName));
    }


    public static bool GetRequiredBoolean(
        this IDataReader reader,
        string columnName)
    {
        return reader.GetBoolean(
            reader.GetOrdinal(columnName));
    }


    public static decimal GetRequiredDecimal(
        this IDataReader reader,
        string columnName)
    {
        return reader.GetDecimal(
            reader.GetOrdinal(columnName));
    }


    public static T GetRequiredEnum<T>(
        this IDataReader reader,
        string columnName)
        where T : struct, Enum
    {
        int ordinal = reader.GetOrdinal(columnName);

        int value = reader.GetInt32(ordinal);

        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new InvalidOperationException(
                $"The value '{value}' is not a valid member of enum '{typeof(T).Name}'.");
        }

        return (T)Enum.ToObject(typeof(T), value);
    }

    public static string? GetNullableString(
    this IDataReader reader,
    string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetString(ordinal);
    }

    public static DateTime? GetNullableDateTime(
    this IDataReader reader,
    string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetDateTime(ordinal);
    }

    public static int? GetNullableInt32(
    this IDataReader reader,
    string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetInt32(ordinal);
    }

}