using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class BlockCompletion : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ScheduleBlockId { get; set; }
    public ScheduleBlock ScheduleBlock { get; set; } = null!;
    public DateOnly Date { get; set; }
    public bool IsDone { get; set; }
}
