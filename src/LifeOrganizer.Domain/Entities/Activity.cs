using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class Activity : BaseEntity
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public int ReminderOffsetMinutes { get; set; } 
}
