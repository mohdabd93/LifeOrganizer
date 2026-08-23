namespace LifeOrganizer.Application.Features.Settings;

public class AppSettingsDto
{
    public bool NotifyGym { get; set; }
    public bool NotifyLanguage { get; set; }
    public bool NotifyActivity { get; set; }
    public bool NotifyWork { get; set; }
    public bool NotifySupplements { get; set; }
    public int DefaultReminderMinutes { get; set; }
    public string? GymLocation { get; set; }
}
