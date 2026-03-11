using System.Data;
using Microsoft.Data.SqlClient;

namespace OkosDobozWeb.Services;

public sealed class ResultsRepository : IResultsRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly Lazy<Task<string>> _fullTableName;

    public ResultsRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _fullTableName = new Lazy<Task<string>>(ResolveTableNameAsync);
    }

    public async Task SaveResultAsync(string exerciseId, int score, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(exerciseId))
        {
            throw new ArgumentException("exerciseId is required", nameof(exerciseId));
        }

        if (score < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(score), "score must be >= 0");
        }

        var fullTableName = await _fullTableName.Value;

        var sql = $"INSERT INTO {fullTableName} ([exerciseid], [score]) VALUES (@exerciseid, @score);";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@exerciseid", SqlDbType.NVarChar, 50) { Value = exerciseId });
        command.Parameters.Add(new SqlParameter("@score", SqlDbType.Int) { Value = score });

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task<string> ResolveTableNameAsync()
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();

        var (schemaName, tableName) = await ResolveResultsTableAsync(connection);
        return $"[{schemaName}].[{tableName}]";
    }

    private static async Task<(string SchemaName, string TableName)> ResolveResultsTableAsync(SqlConnection connection)
    {
        const string sql = """
SELECT TOP 1 s.name AS SchemaName, o.name AS TableName
FROM sys.objects o
JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.type = 'U' AND LOWER(o.name) = @name
ORDER BY s.name
""";

        await using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128) { Value = "results" });

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException("Could not find a table named 'results' in the database.");
        }

        return (reader.GetString(0), reader.GetString(1));
    }
}
