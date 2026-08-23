using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class GymSession : BaseEntity
{
    public Guid UserId { get; set; }

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; } 
    public int? DurationMinutes { get; set; }
}
