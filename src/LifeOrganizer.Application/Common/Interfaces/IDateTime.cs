namespace LifeOrganizer.Application.Common.Interfaces;

public interface IDateTime
{
    DateOnly Today { get; }
    DateTime Now { get; }
}
