namespace LifeOrganizer.Application.Features.Supplements;

public class SupplementDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Dose { get; set; }
    public string Unit { get; set; } = string.Empty;
    public TimeOnly Time { get; set; }
    public bool ReminderEnabled { get; set; }
}
