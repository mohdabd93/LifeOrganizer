namespace LifeOrganizer.Application.Features.ScheduleBlocks;

public class ScheduleBlockDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string ColorHex { get; set; } = string.Empty;
    public bool DoneToday { get; set; }
}
