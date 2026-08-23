using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Supplements;

public record GetSupplementsQuery : IRequest<List<SupplementDto>>;

public class GetSupplementsQueryHandler : IRequestHandler<GetSupplementsQuery, List<SupplementDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSupplementsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<SupplementDto>> Handle(GetSupplementsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Supplements.AsNoTracking()
            .OrderBy(s => s.Time)
            .Select(s => new SupplementDto
            {
                Id = s.Id, Name = s.Name, Dose = s.Dose, Unit = s.Unit,
                Time = s.Time, ReminderEnabled = s.ReminderEnabled
            })
            .ToListAsync(cancellationToken);
    }
}
