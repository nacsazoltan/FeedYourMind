using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public interface IVideoRepository
{
    Task<List<VideoItem>> GetVideosAsync(string culture, string topic, CancellationToken cancellationToken = default);
    Task<List<VideoItem>> GetVideosByLanguageAsync(string culture, CancellationToken cancellationToken = default);
}
