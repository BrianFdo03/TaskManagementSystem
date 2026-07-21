using System.Xml.Linq;

namespace TaskManagement.Infrastructure.QueryManagement;

public class SqlQueryService : ISqlQueryService
{
    private readonly Dictionary<string, Dictionary<string, string>> _queries;

    public SqlQueryService()
    {
        _queries = new Dictionary<string, Dictionary<string, string>>();

        LoadQueries();
    }

    private void LoadQueries()
    {
        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QueryManagement/Queries");

        if (!Directory.Exists(basePath))
            throw new DirectoryNotFoundException($"Query folder not found: {basePath}");

        var files = Directory.GetFiles(basePath, "*.xml");

        foreach (var file in files)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);

            var doc = XDocument.Load(file);

            var queryDict = new Dictionary<string, string>();

            foreach (var element in doc.Descendants("Query"))
            {
                var name = element.Attribute("name")?.Value;
                var sql = element.Value?.Trim();

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(sql))
                {
                    queryDict[name] = sql;
                }
            }

            _queries[fileName] = queryDict;
        }
    }

    public string Get(string file, string queryName)
    {
        if (!_queries.TryGetValue(file, out var queries))
        {
            throw new FileNotFoundException(
                $"SQL query file '{file}' was not found.");
        }

        if (!queries.TryGetValue(queryName, out var sql))
        {
            throw new KeyNotFoundException(
                $"SQL query '{queryName}' was not found in '{file}'.");
        }

        return sql;
    }
}