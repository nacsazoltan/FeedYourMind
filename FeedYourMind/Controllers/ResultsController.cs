using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Services;

namespace OkosDobozWeb.Controllers;

[ApiController]
[Route("api/results")]
public sealed class ResultsController : ControllerBase
{
    private readonly IResultsRepository _resultsRepository;

    public ResultsController(IResultsRepository resultsRepository)
    {
        _resultsRepository = resultsRepository;
    }

    public sealed record CreateResultRequest(string ExerciseId, int Score);

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateResultRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.ExerciseId))
        {
            return BadRequest(new { error = "ExerciseId is required" });
        }

        if (request.Score < 0)
        {
            return BadRequest(new { error = "Score must be >= 0" });
        }

        await _resultsRepository.SaveResultAsync(request.ExerciseId, request.Score, cancellationToken);
        return Ok(new { saved = true });
    }
}
