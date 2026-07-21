using Microsoft.Data.SqlClient;

namespace TaskManagement.Infrastructure.Database;

public interface IDbConnectionFactory
{
    SqlConnection CreateConnection();
}