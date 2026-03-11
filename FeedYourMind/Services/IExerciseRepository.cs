using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public interface IExerciseRepository
{
    Task<List<ExerciseItem>> GetExercisesAsync(string culture, string topic, CancellationToken cancellationToken = default);
}
