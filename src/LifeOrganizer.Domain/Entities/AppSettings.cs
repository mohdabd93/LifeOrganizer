using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class AppSettings : BaseEntity
{
    public Guid UserId { get; set; }

    public bool NotifyGym { get; set; } = true;
    public bool NotifyLanguage { get; set; } = true;
    public bool NotifyActivity { get; set; } = true;
    public bool NotifyWork { get; set; } = false;
    public bool NotifySupplements { get; set; } = true;
    public int DefaultReminderMinutes { get; set; } = 10;
    public string? GymLocation { get; set; }
}
