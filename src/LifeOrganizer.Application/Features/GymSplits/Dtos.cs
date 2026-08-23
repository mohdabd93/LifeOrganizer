namespace LifeOrganizer.Application.Features.GymSplits;

public class GymSplitDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ExerciseCount { get; set; }
}
