using LifeOrganizer.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Common.Services;

public class ActivityConflictChecker : IActivityConflictChecker
{
    private readonly IApplicationDbContext _context;

    public ActivityConflictChecker(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<string>> FindConflictsAsync(
        DateOnly date,
        TimeOnly time,
        int durationMinutes,
        Guid? excludeActivityId,
        CancellationToken cancellationToken)
    {
        var start = time;
        var end = time.AddMinutes(durationMinutes);
        var conflicts = new List<string>();

         var blocks = await _context.ScheduleBlocks.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var block in blocks)
        {
            if (Overlaps(start, end, block.StartTime, block.EndTime))
            {
                conflicts.Add(block.Name);
            }
        }

         var sameDayActivities = await _context.Activities
            .AsNoTracking()
            .Where(a => a.Date == date && (excludeActivityId == null || a.Id != excludeActivityId))
            .ToListAsync(cancellationToken);

        foreach (var activity in sameDayActivities)
        {
            var otherEnd = activity.Time.AddMinutes(activity.DurationMinutes);
            if (Overlaps(start, end, activity.Time, otherEnd))
            {
                conflicts.Add(activity.Name);
            }
        }

        return conflicts;
    }

    private static bool Overlaps(TimeOnly startA, TimeOnly endA, TimeOnly startB, TimeOnly endB)
        => startA < endB && startB < endA;
}
