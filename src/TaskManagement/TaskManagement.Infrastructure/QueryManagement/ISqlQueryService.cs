namespace TaskManagement.Infrastructure.QueryManagement;

public interface ISqlQueryService
{
    string Get(string file, string queryName);

}