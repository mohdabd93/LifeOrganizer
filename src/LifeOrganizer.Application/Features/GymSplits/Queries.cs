using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.GymSplits;

public record GetGymSplitsQuery : IRequest<List<GymSplitDto>>;

public class GetGymSplitsQueryHandler : IRequestHandler<GetGymSplitsQuery, List<GymSplitDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGymSplitsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<GymSplitDto>> Handle(GetGymSplitsQuery request, CancellationToken cancellationToken)
    {
        return await _context.GymSplits.AsNoTracking()
            .Select(s => new GymSplitDto { Id = s.Id, Name = s.Name, ExerciseCount = s.Exercises.Count })
            .ToListAsync(cancellationToken);
    }
}
