using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public interface IPageRepository
{
    Task<PageItem?> GetPageAsync(string culture, string page, CancellationToken cancellationToken = default);
}
