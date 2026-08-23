using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class ExerciseSet : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ExerciseLogId { get; set; }
    public ExerciseLog ExerciseLog { get; set; } = null!;
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public int SetNumber { get; set; }
}
