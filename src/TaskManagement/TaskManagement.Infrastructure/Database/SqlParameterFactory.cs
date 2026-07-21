using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Infrastructure.Database
{
    public static class SqlParameterFactory
    {
        public static SqlParameter Create(
            string name,
            object? value)
        {
            return new SqlParameter(
                name,
                value ?? DBNull.Value);
        }
    }
}
