using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Exercises;

public record GetExercisesBySplitQuery(Guid GymSplitId) : IRequest<List<ExerciseDto>>;

public class GetExercisesBySplitQueryHandler : IRequestHandler<GetExercisesBySplitQuery, List<ExerciseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesBySplitQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ExerciseDto>> Handle(GetExercisesBySplitQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _context.Exercises.AsNoTracking()
            .Where(e => e.GymSplitId == request.GymSplitId)
            .Include(e => e.Logs).ThenInclude(l => l.Sets)
            .ToListAsync(cancellationToken);

        return exercises.Select(e =>
        {
            var lastLog = e.Logs.OrderByDescending(l => l.Date).FirstOrDefault();
            return new ExerciseDto
            {
                Id = e.Id,
                GymSplitId = e.GymSplitId,
                Name = e.Name,
                TargetSets = e.TargetSets,
                NextTargetWeightKg = e.NextTargetWeightKg,
                CurrentWeightKg = lastLog?.Sets.Count > 0 ? lastLog.Sets.Max(s => s.WeightKg) : null
            };
        }).ToList();
    }
}
