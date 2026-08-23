namespace LifeOrganizer.Application.Features.Activities;

public class ActivityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int DurationMinutes { get; set; }
    public int ReminderOffsetMinutes { get; set; }
}
