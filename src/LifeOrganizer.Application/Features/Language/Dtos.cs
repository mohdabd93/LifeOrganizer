namespace LifeOrganizer.Application.Features.Language;

public class LanguageWordDto
{
    public Guid Id { get; set; }
    public string TargetLanguageText { get; set; } = string.Empty;
    public string TranslationText { get; set; } = string.Empty;
}

public class LanguageProgressDto
{
    public string CurrentLevel { get; set; } = string.Empty;
    public string TargetLevel { get; set; } = string.Empty;
    public int ProgressPercent { get; set; }
}
