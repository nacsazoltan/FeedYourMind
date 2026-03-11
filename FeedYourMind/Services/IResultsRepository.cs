namespace OkosDobozWeb.Services;

public interface IResultsRepository
{
    Task SaveResultAsync(string exerciseId, int score, CancellationToken cancellationToken = default);
}
