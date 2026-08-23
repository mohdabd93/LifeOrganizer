using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Application.Common.Services;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Activities;
 
public class ActivityMutationResult
{
    public Guid? Id { get; set; }
    public bool Success { get; set; }
    public IReadOnlyList<string> Conflicts { get; set; } = Array.Empty<string>();
}

public record CreateActivityCommand(
    string Name,
    DayOfWeek Day,
    DateOnly Date,
    TimeOnly Time,
    int DurationMinutes,
    int ReminderOffsetMinutes,
    bool ForceSave = false) : IRequest<ActivityMutationResult>;

public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DurationMinutes).GreaterThan(0).LessThanOrEqualTo(24 * 60);
        RuleFor(x => x.ReminderOffsetMinutes).GreaterThanOrEqualTo(0);
    }
}

public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, ActivityMutationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IActivityConflictChecker _conflictChecker;
    private readonly ICurrentUserService _currentUser;

    public CreateActivityCommandHandler(
        IApplicationDbContext context,
        IActivityConflictChecker conflictChecker,
        ICurrentUserService currentUser)
    {
        _context = context;
        _conflictChecker = conflictChecker;
        _currentUser = currentUser;
    }

    public async Task<ActivityMutationResult> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        var conflicts = await _conflictChecker.FindConflictsAsync(
            request.Date, request.Time, request.DurationMinutes, excludeActivityId: null, cancellationToken);

        if (conflicts.Count > 0 && !request.ForceSave)
        {
            return new ActivityMutationResult { Success = false, Conflicts = conflicts };
        }

        var entity = new Activity
        {
            UserId = _currentUser.UserId!.Value,
            Name = request.Name,
            Day = request.Day,
            Date = request.Date,
            Time = request.Time,
            DurationMinutes = request.DurationMinutes,
            ReminderOffsetMinutes = request.ReminderOffsetMinutes
        };

        _context.Activities.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new ActivityMutationResult { Id = entity.Id, Success = true, Conflicts = conflicts };
    }
}

public record UpdateActivityCommand(
    Guid Id,
    string Name,
    DayOfWeek Day,
    DateOnly Date,
    TimeOnly Time,
    int DurationMinutes,
    int ReminderOffsetMinutes,
    bool ForceSave = false) : IRequest<ActivityMutationResult>;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
{
    public UpdateActivityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DurationMinutes).GreaterThan(0).LessThanOrEqualTo(24 * 60);
        RuleFor(x => x.ReminderOffsetMinutes).GreaterThanOrEqualTo(0);
    }
}

public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, ActivityMutationResult>
{
    private readonly IApplicationDbContext _context;
    private readonly IActivityConflictChecker _conflictChecker;

    public UpdateActivityCommandHandler(IApplicationDbContext context, IActivityConflictChecker conflictChecker)
    {
        _context = context;
        _conflictChecker = conflictChecker;
    }

    public async Task<ActivityMutationResult> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (entity is null)
        {
            return new ActivityMutationResult { Success = false };
        }

        var conflicts = await _conflictChecker.FindConflictsAsync(
            request.Date, request.Time, request.DurationMinutes, excludeActivityId: request.Id, cancellationToken);

        if (conflicts.Count > 0 && !request.ForceSave)
        {
            return new ActivityMutationResult { Success = false, Conflicts = conflicts };
        }

        entity.Name = request.Name;
        entity.Day = request.Day;
        entity.Date = request.Date;
        entity.Time = request.Time;
        entity.DurationMinutes = request.DurationMinutes;
        entity.ReminderOffsetMinutes = request.ReminderOffsetMinutes;

        await _context.SaveChangesAsync(cancellationToken);

        return new ActivityMutationResult { Id = entity.Id, Success = true, Conflicts = conflicts };
    }
}

public record DeleteActivityCommand(Guid Id) : IRequest;

public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteActivityCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Activities.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.Activities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
