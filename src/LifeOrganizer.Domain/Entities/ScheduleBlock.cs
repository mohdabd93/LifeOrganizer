using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;
 
public class ScheduleBlock : BaseEntity
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ColorHex { get; set; } = "#0A84FF";
    public int SortOrder { get; set; }

    public ICollection<BlockCompletion> Completions { get; set; } = new List<BlockCompletion>();
}
