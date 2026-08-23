using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class GymSplit : BaseEntity
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
