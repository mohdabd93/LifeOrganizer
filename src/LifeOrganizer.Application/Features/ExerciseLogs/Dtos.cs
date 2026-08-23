namespace LifeOrganizer.Application.Features.ExerciseLogs;

public class ExerciseSetDto
{
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
}

public class ExerciseLogDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public DateOnly Date { get; set; }
    public List<ExerciseSetDto> Sets { get; set; } = new();
}
