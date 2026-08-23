using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.ExerciseLogs;
 
public record GetExerciseHistoryQuery(Guid ExerciseId) : IRequest<List<ExerciseLogDto>>;

public class GetExerciseHistoryQueryHandler : IRequestHandler<GetExerciseHistoryQuery, List<ExerciseLogDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExerciseHistoryQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ExerciseLogDto>> Handle(GetExerciseHistoryQuery request, CancellationToken cancellationToken)
    {
        var logs = await _context.ExerciseLogs.AsNoTracking()
            .Where(l => l.ExerciseId == request.ExerciseId)
            .Include(l => l.Sets)
            .OrderBy(l => l.Date)
            .ToListAsync(cancellationToken);

        return logs.Select(l => new ExerciseLogDto
        {
            Id = l.Id,
            ExerciseId = l.ExerciseId,
            Date = l.Date,
            Sets = l.Sets
                .OrderBy(s => s.SetNumber)
                .Select(s => new ExerciseSetDto { WeightKg = s.WeightKg, Reps = s.Reps })
                .ToList()
        }).ToList();
    }
}
