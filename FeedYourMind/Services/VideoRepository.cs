using System.Data;
using Microsoft.Data.SqlClient;
using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public sealed class VideoRepository : IVideoRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    // Cache resolved table + column mapping for the app lifetime
    private readonly Lazy<Task<string>> _fullTableName;

    public VideoRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _fullTableName = new Lazy<Task<string>>(ResolveTableNameAsync);
    }

    public async Task<List<VideoItem>> GetVideosAsync(string culture, string topic, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            topic = "basic";
        }

        var normalizedLanguage = NormalizeLanguage(culture);
        var fullTableName = await _fullTableName.Value;

        var sql = $"SELECT [topic] AS Topic, [videotitle] AS Title, [youtubeid] AS VideoId FROM {fullTableName} WHERE [status] = 1 AND [topic] = @topic AND [language] = @language ORDER BY [id]";

        var results = new List<VideoItem>();

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@topic", topic);
        command.Parameters.AddWithValue("@language", normalizedLanguage);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new VideoItem
            {
                Topic = reader.GetString(reader.GetOrdinal("Topic")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                VideoId = reader.GetString(reader.GetOrdinal("VideoId")),
            });
        }

        return results;
    }

    public async Task<List<VideoItem>> GetVideosByLanguageAsync(string culture, CancellationToken cancellationToken = default)
    {
        var normalizedLanguage = NormalizeLanguage(culture);
        var fullTableName = await _fullTableName.Value;

        var sql = $"SELECT [topic] AS Topic, [videotitle] AS Title, [youtubeid] AS VideoId FROM {fullTableName} WHERE [status] = 1 AND [language] = @language ORDER BY [id]";

        var results = new List<VideoItem>();

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@language", normalizedLanguage);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new VideoItem
            {
                Topic = reader.GetString(reader.GetOrdinal("Topic")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                VideoId = reader.GetString(reader.GetOrdinal("VideoId")),
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


        var (schemaName, tableName) = await ResolveVideoListTableAsync(connection);
        return $"[{schemaName}].[{tableName}]";
    }

    private static async Task<(string SchemaName, string TableName)> ResolveVideoListTableAsync(SqlConnection connection)
    {
        // Looks up any user table named "videolist" (case-insensitive), regardless of schema.
        const string sql = """
SELECT TOP 1 s.name AS SchemaName, o.name AS TableName
FROM sys.objects o
JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.type = 'U' AND LOWER(o.name) = @name
ORDER BY s.name
""";

        await using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128) { Value = "videolist" });

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException("Could not find a table named 'videolist' in the database.");
        }

        return (reader.GetString(0), reader.GetString(1));
    }
}
