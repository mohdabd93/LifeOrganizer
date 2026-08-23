using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Activities;

public record GetActivitiesQuery(DateOnly? FromDate = null) : IRequest<List<ActivityDto>>;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, List<ActivityDto>>
{
    private readonly IApplicationDbContext _context;

    public GetActivitiesQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ActivityDto>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Activities.AsNoTracking().AsQueryable();

        if (request.FromDate is not null)
        {
            query = query.Where(a => a.Date >= request.FromDate);
        }

        return await query
            .OrderBy(a => a.Date).ThenBy(a => a.Time)
            .Select(a => new ActivityDto
            {
                Id = a.Id,
                Name = a.Name,
                Day = a.Day,
                Date = a.Date,
                Time = a.Time,
                DurationMinutes = a.DurationMinutes,
                ReminderOffsetMinutes = a.ReminderOffsetMinutes
            })
            .ToListAsync(cancellationToken);
    }
}
