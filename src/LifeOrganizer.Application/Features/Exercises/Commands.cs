using FluentValidation;
using LifeOrganizer.Application.Common.Exceptions;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Exercises;

public record CreateExerciseCommand(Guid GymSplitId, string Name, string TargetSets) : IRequest<Guid>;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
}

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateExerciseCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = new Exercise
        {
            UserId = _currentUser.UserId!.Value,
            GymSplitId = request.GymSplitId,
            Name = request.Name,
            TargetSets = request.TargetSets
        };
        _context.Exercises.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record UpdateExerciseTargetCommand(Guid Id, decimal? NextTargetWeightKg) : IRequest;

public class UpdateExerciseTargetCommandHandler : IRequestHandler<UpdateExerciseTargetCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseTargetCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateExerciseTargetCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Exercise), request.Id);

        entity.NextTargetWeightKg = request.NextTargetWeightKg;
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteExerciseCommand(Guid Id) : IRequest;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.Exercises.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
