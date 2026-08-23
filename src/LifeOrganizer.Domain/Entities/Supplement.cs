using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class Supplement : BaseEntity
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Dose { get; set; }
    public string Unit { get; set; } = "GM";
    public TimeOnly Time { get; set; }
    public bool ReminderEnabled { get; set; } = true;
}
