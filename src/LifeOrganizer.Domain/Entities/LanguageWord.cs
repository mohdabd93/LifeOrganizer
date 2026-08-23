using LifeOrganizer.Domain.Common;

namespace LifeOrganizer.Domain.Entities;

public class LanguageWord : BaseEntity
{
    public Guid UserId { get; set; }

    public string TargetLanguageText { get; set; } = string.Empty;  
    public string TranslationText { get; set; } = string.Empty;    
}
