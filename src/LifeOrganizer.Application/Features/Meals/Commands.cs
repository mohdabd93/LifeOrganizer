using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Meals;

public record CreateMealCommand(string Name, int Calories, DateOnly Date) : IRequest<Guid>;

public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
{
    public CreateMealCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Calories).GreaterThanOrEqualTo(0);
    }
}

public class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateMealCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        var entity = new Meal
        {
            UserId = _currentUser.UserId!.Value,
            Name = request.Name, Calories = request.Calories, Date = request.Date
        };
        _context.Meals.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record DeleteMealCommand(Guid Id) : IRequest;

public class DeleteMealCommandHandler : IRequestHandler<DeleteMealCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMealCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteMealCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Meals.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.Meals.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
