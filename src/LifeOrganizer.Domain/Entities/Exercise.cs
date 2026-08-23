using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class Exercise : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid GymSplitId { get; set; }
    public GymSplit GymSplit { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string TargetSets { get; set; } = string.Empty;
    public decimal? NextTargetWeightKg { get; set; }

    public ICollection<ExerciseLog> Logs { get; set; } = new List<ExerciseLog>();
}
