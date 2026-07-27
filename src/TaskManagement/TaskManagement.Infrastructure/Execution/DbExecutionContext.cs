using Microsoft.Data.SqlClient;

namespace TaskManagement.Infrastructure.Execution;

public class DbExecutionContext
{
    public SqlConnection? Connection { get; set; }
    public SqlTransaction? Transaction { get; set; }

    public bool HasTransaction => Transaction != null;
}