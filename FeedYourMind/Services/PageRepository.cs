using System.Data;
using Microsoft.Data.SqlClient;
using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public sealed class PageRepository : IPageRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly Lazy<Task<string>> _fullTableName;

    public PageRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        _fullTableName = new Lazy<Task<string>>(ResolveTableNameAsync);
    }

    public async Task<PageItem?> GetPageAsync(string culture, string page, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(page))
        {
            return null;
        }

        var normalizedLanguage = NormalizeLanguage(culture);
        var fullTableName = await _fullTableName.Value;

        var sql = $"SELECT TOP (1) [page] AS Page, [name] AS Name, [htmltext] AS HtmlText FROM {fullTableName} WHERE [page] = @page AND [language] = @language ORDER BY [id] DESC";

        await using var connection = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.Add(new SqlParameter("@page", SqlDbType.NVarChar, 100) { Value = page });
        command.Parameters.Add(new SqlParameter("@language", SqlDbType.Char, 2) { Value = normalizedLanguage });

        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            // Optional fallback to HU if specific language content is missing.
            if (!string.Equals(normalizedLanguage, "hu", StringComparison.OrdinalIgnoreCase))
            {
                return await GetPageAsync("hu", page, cancellationToken);
            }

            return null;
        }

        return new PageItem
        {
            Page = reader.GetString(reader.GetOrdinal("Page")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            HtmlText = reader.GetString(reader.GetOrdinal("HtmlText"))
        };
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

        var (schemaName, tableName) = await ResolvePageListTableAsync(connection);
        return $"[{schemaName}].[{tableName}]";
    }

    private static async Task<(string SchemaName, string TableName)> ResolvePageListTableAsync(SqlConnection connection)
    {
        const string sql = """
SELECT TOP 1 s.name AS SchemaName, o.name AS TableName
FROM sys.objects o
JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE o.type = 'U' AND LOWER(o.name) = @name
ORDER BY s.name
""";

        await using var cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 128) { Value = "pagelist" });

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
        {
            throw new InvalidOperationException("Could not find a table named 'pagelist' in the database.");
        }

        return (reader.GetString(0), reader.GetString(1));
    }
}
