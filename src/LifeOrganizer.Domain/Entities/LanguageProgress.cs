using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class LanguageProgress : BaseEntity
{
    public Guid UserId { get; set; }

    public string CurrentLevel { get; set; } = "A1";
    public string TargetLevel { get; set; } = "A2";
    public int ProgressPercent { get; set; }
}
