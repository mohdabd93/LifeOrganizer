using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class ExerciseLog : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public DateOnly Date { get; set; }

    public ICollection<ExerciseSet> Sets { get; set; } = new List<ExerciseSet>();
}
