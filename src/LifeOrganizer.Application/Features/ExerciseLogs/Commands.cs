using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;

namespace LifeOrganizer.Application.Features.ExerciseLogs;

public record CreateExerciseLogCommand(Guid ExerciseId, DateOnly Date, List<ExerciseSetDto> Sets)
    : IRequest<Guid>;

public class CreateExerciseLogCommandValidator : AbstractValidator<CreateExerciseLogCommand>
{
    public CreateExerciseLogCommandValidator()
    {
        RuleFor(x => x.Sets).NotEmpty().WithMessage("One set at least is required");
        RuleForEach(x => x.Sets).ChildRules(set =>
        {
            set.RuleFor(s => s.WeightKg).GreaterThanOrEqualTo(0);
            set.RuleFor(s => s.Reps).GreaterThan(0);
        });
    }
}

public class CreateExerciseLogCommandHandler : IRequestHandler<CreateExerciseLogCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateExerciseLogCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateExerciseLogCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId!.Value;
        var log = new ExerciseLog { UserId = userId, ExerciseId = request.ExerciseId, Date = request.Date };

        var setNumber = 1;
        foreach (var set in request.Sets)
        {
            log.Sets.Add(new ExerciseSet
            {
                UserId = userId,
                WeightKg = set.WeightKg,
                Reps = set.Reps,
                SetNumber = setNumber++
            });
        }

        _context.ExerciseLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);
        return log.Id;
    }
}
