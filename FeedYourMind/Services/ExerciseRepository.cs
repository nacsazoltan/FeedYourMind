using System.Data;
using Microsoft.Data.SqlClient;
using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public sealed class ExerciseRepository : IExerciseRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly Lazy<Task<string>> _fullTableName;

    public ExerciseRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _fullTableName = new Lazy<Task<string>>(ResolveTableNameAsync);
    }

    public async Task<List<ExerciseItem>> GetExercisesAsync(string culture, string topic, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            topic = "basic";
        }

        var normalizedLanguage = NormalizeLanguage(culture);
        var fullTableName = await _fullTableName.Value;

        var sql = $"SELECT [topic] AS Topic, [exercisetitle] AS Name, [exerciseid] AS ExerciseId, [grade] AS Grade FROM {fullTableName} WHERE [status] = 1 AND [topic] = @topic AND [language] = @language ORDER BY [id]";

        var results = new List<ExerciseItem>();

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@topic", SqlDbType.NVarChar, 50) { Value = topic });
        command.Parameters.Add(new SqlParameter("@language", SqlDbType.Char, 2) { Value = normalizedLanguage });

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            int? grade = null;
            var gradeOrdinal = reader.GetOrdinal("Grade");
            if (!reader.IsDBNull(gradeOrdinal))
            {
                grade = Convert.ToInt32(reader.GetValue(gradeOrdinal));
            }

            results.Add(new ExerciseItem
            {
                Topic = reader.GetString(reader.GetOrdinal("Topic")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ExerciseId = reader.GetString(reader.GetOrdinal("ExerciseId")),
                Grade = grade
            });
        }

        return results;
    }

    public async Task<List<ExerciseItem>> GetExercisesByLanguageAsync(string culture, CancellationToken cancellationToken = default)
    {
        var normalizedLanguage = NormalizeLanguage(culture);
        var fullTableName = await _fullTableName.Value;

        var sql = $"SELECT [topic] AS Topic, [exercisetitle] AS Name, [exerciseid] AS ExerciseId, [grade] AS Grade FROM {fullTableName} WHERE [status] = 1 AND [language] = @language ORDER BY [id]";

        var results = new List<ExerciseItem>();

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@language", SqlDbType.Char, 2) { Value = normalizedLanguage });

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            int? grade = null;
            var gradeOrdinal = reader.GetOrdinal("Grade");
            if (!reader.IsDBNull(gradeOrdinal))
            {
                grade = Convert.ToInt32(reader.GetValue(gradeOrdinal));
            }

            results.Add(new ExerciseItem
            {
                Topic = reader.GetString(reader.GetOrdinal("Topic")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                ExerciseId = reader.GetString(reader.GetOrdinal("ExerciseId")),
                Grade = grade
            });
        }

        return results;
    }

    private static string NormalizeLanguage(string? culture)
    {
        if (string.IsNullOrWhiteSpace(culture))
        {
            return "hu";
        }

        var dashIndex = culture.IndexOf('-');
        var normalized = dashIndex > 0 ? culture[..dashIndex] : culture;

        // App uses "cs" (Czech) culture, DB stores it as "cz".
        if (string.Equals(normalized, "cs", StringComparison.OrdinalIgnoreCase))
        {
            return "cz";
        }

        return normalized;
    }

    private async Task<string> ResolveTableNameAsync()
    {
        await using var connection = await _connectionFactory.CreateOpenConnectionAsync();

        var (schemaName, tableName) = await ResolveExerciseListTableAsync(connection);
        return $"[{schemaName}].[{tableName}]";
    }

    private static async Task<(string SchemaName, string TableName)> ResolveExerciseListTableAsync(SqlConnection connection)
    {
        const string sql = """
SELECT TOP 1 s.name AS SchemaName, o.name AS TableName
FROM sys.objects o
JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.type = 'U' AND LOWER(o.name) = @name
ORDER BY s.name
""";

        await using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128) { Value = "exerciselist" });

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException("Could not find a table named 'exerciselist' in the database.");
        }

        return (reader.GetString(0), reader.GetString(1));
    }
}
