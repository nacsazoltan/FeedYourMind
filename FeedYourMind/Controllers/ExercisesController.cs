using Microsoft.AspNetCore.Mvc;
using OkosDobozWeb.Services;

namespace OkosDobozWeb.Controllers;

[ApiController]
[Route("api")]
public sealed class ExercisesController : ControllerBase
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExercisesController(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    [HttpGet("getexercises")]
    public async Task<IActionResult> GetExercises([FromQuery] string language, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            return BadRequest(new { error = "language is required" });
        }

        var exercises = await _exerciseRepository.GetExercisesByLanguageAsync(language, cancellationToken);

        var response = exercises.Select(exercise => new
        {
            exercisetitle = exercise.Name,
            exerciseid = exercise.ExerciseId,
            topic = exercise.Topic,
            grade = exercise.Grade
        });

        return Ok(response);
    }
}
