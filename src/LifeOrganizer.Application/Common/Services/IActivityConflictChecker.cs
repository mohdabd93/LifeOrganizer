namespace LifeOrganizer.Application.Common.Services;

public interface IActivityConflictChecker
{ 
    Task<IReadOnlyList<string>> FindConflictsAsync(
        DateOnly date,
        TimeOnly time,
        int durationMinutes,
        Guid? excludeActivityId,
        CancellationToken cancellationToken);
}
