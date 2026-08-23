using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.ScheduleBlocks;

public record GetScheduleBlocksQuery : IRequest<List<ScheduleBlockDto>>;

public class GetScheduleBlocksQueryHandler : IRequestHandler<GetScheduleBlocksQuery, List<ScheduleBlockDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public GetScheduleBlocksQueryHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<List<ScheduleBlockDto>> Handle(GetScheduleBlocksQuery request, CancellationToken cancellationToken)
    {
        var today = _dateTime.Today;

        var blocks = await _context.ScheduleBlocks.AsNoTracking()
            .OrderBy(b => b.SortOrder)
            .ToListAsync(cancellationToken);

        var todaysCompletions = await _context.BlockCompletions.AsNoTracking()
            .Where(c => c.Date == today)
            .ToDictionaryAsync(c => c.ScheduleBlockId, c => c.IsDone, cancellationToken);

        return blocks.Select(b => new ScheduleBlockDto
        {
            Id = b.Id,
            Name = b.Name,
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            ColorHex = b.ColorHex,
            DoneToday = todaysCompletions.TryGetValue(b.Id, out var done) && done
        }).ToList();
    }
}
