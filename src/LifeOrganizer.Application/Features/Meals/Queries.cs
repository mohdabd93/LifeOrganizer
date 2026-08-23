using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Meals;

public record GetMealsQuery(DateOnly Date) : IRequest<List<MealDto>>;

public class GetMealsQueryHandler : IRequestHandler<GetMealsQuery, List<MealDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMealsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<MealDto>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Meals.AsNoTracking()
            .Where(m => m.Date == request.Date)
            .Select(m => new MealDto { Id = m.Id, Name = m.Name, Calories = m.Calories, Date = m.Date })
            .ToListAsync(cancellationToken);
    }
}
