using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class Meal : BaseEntity
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Calories { get; set; }
    public DateOnly Date { get; set; }
}
