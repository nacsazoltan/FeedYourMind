using OkosDobozWeb.Models;

namespace OkosDobozWeb.Services;

public interface IExerciseRepository
{
    Task<List<ExerciseItem>> GetExercisesAsync(string culture, string topic, CancellationToken cancellationToken = default);
    Task<List<ExerciseItem>> GetExercisesByLanguageAsync(string culture, CancellationToken cancellationToken = default);
}
