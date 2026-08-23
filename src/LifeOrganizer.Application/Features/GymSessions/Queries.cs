using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.GymSessions;

public record GetGymSessionsQuery(int Take = 20) : IRequest<List<GymSessionDto>>;

public class GetGymSessionsQueryHandler : IRequestHandler<GetGymSessionsQuery, List<GymSessionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGymSessionsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<GymSessionDto>> Handle(GetGymSessionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.GymSessions.AsNoTracking()
            .OrderByDescending(s => s.Date).ThenByDescending(s => s.StartTime)
            .Take(request.Take)
            .Select(s => new GymSessionDto
            {
                Id = s.Id,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                DurationMinutes = s.DurationMinutes
            })
            .ToListAsync(cancellationToken);
    }
}
 
public record GetActiveGymSessionQuery : IRequest<GymSessionDto?>;

public class GetActiveGymSessionQueryHandler : IRequestHandler<GetActiveGymSessionQuery, GymSessionDto?>
{
    private readonly IApplicationDbContext _context;

    public GetActiveGymSessionQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<GymSessionDto?> Handle(GetActiveGymSessionQuery request, CancellationToken cancellationToken)
    {
        var session = await _context.GymSessions.AsNoTracking()
            .Where(s => s.EndTime == null)
            .OrderByDescending(s => s.Date).ThenByDescending(s => s.StartTime)
            .FirstOrDefaultAsync(cancellationToken);

        return session is null ? null : new GymSessionDto
        {
            Id = session.Id,
            Date = session.Date,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            DurationMinutes = session.DurationMinutes
        };
    }
}
