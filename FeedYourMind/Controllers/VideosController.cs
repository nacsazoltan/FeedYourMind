using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Services;

namespace OkosDobozWeb.Controllers;

[ApiController]
[Route("api")]
public sealed class VideosController : ControllerBase
{
    private readonly IVideoRepository _videoRepository;

    public VideosController(IVideoRepository videoRepository)
    {
        _videoRepository = videoRepository;
    }

    [HttpGet("getvideos")]
    public async Task<IActionResult> GetVideos([FromQuery] string language, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return BadRequest(new { error = "language is required" });
        }

        var videos = await _videoRepository.GetVideosByLanguageAsync(language, cancellationToken);

        var response = videos.Select(video => new
        {
            videotitle = video.Title,
            youtubeid = video.VideoId,
            topic = video.Topic
        });

        return Ok(response);
    }
}
