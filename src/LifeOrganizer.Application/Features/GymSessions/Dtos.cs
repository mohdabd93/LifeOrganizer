namespace LifeOrganizer.Application.Features.GymSessions;

public class GymSessionDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public int? DurationMinutes { get; set; }
}
