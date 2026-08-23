namespace LifeOrganizer.Application.Features.Exercises;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public Guid GymSplitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TargetSets { get; set; } = string.Empty;
    public decimal? NextTargetWeightKg { get; set; }
    public decimal? CurrentWeightKg { get; set; }  
}
