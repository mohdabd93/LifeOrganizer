using FluentValidation;
using LifeOrganizer.Application.Common.Exceptions;
using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.ScheduleBlocks;

public record UpdateScheduleBlockCommand(Guid Id, string Name, TimeOnly StartTime, TimeOnly EndTime, string ColorHex)
    : IRequest;

public class UpdateScheduleBlockCommandValidator : AbstractValidator<UpdateScheduleBlockCommand>
{
    public UpdateScheduleBlockCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class UpdateScheduleBlockCommandHandler : IRequestHandler<UpdateScheduleBlockCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateScheduleBlockCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(UpdateScheduleBlockCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ScheduleBlocks.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.ScheduleBlock), request.Id);

        entity.Name = request.Name;
        entity.StartTime = request.StartTime;
        entity.EndTime = request.EndTime;
        entity.ColorHex = request.ColorHex;

        await _context.SaveChangesAsync(cancellationToken);
    }
} 
public record CreateScheduleBlockCommand(string Name, TimeOnly StartTime, TimeOnly EndTime, string ColorHex)
    : IRequest<Guid>;

public class CreateScheduleBlockCommandValidator : AbstractValidator<CreateScheduleBlockCommand>
{
    public CreateScheduleBlockCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class CreateScheduleBlockCommandHandler : IRequestHandler<CreateScheduleBlockCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateScheduleBlockCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateScheduleBlockCommand request, CancellationToken cancellationToken)
    {
        var maxSortOrder = await _context.ScheduleBlocks.MaxAsync(b => (int?)b.SortOrder, cancellationToken) ?? 0;

        var entity = new Domain.Entities.ScheduleBlock
        {
            UserId = _currentUser.UserId!.Value,
            Name = request.Name,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            ColorHex = request.ColorHex,
            SortOrder = maxSortOrder + 1
        };

        _context.ScheduleBlocks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record DeleteScheduleBlockCommand(Guid Id) : IRequest;

public class DeleteScheduleBlockCommandHandler : IRequestHandler<DeleteScheduleBlockCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteScheduleBlockCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteScheduleBlockCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ScheduleBlocks
            .Include(b => b.Completions)
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.BlockCompletions.RemoveRange(entity.Completions);
        _context.ScheduleBlocks.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record ToggleBlockCompletionCommand(Guid ScheduleBlockId, DateOnly Date, bool IsDone) : IRequest;

public class ToggleBlockCompletionCommandHandler : IRequestHandler<ToggleBlockCompletionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ToggleBlockCompletionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(ToggleBlockCompletionCommand request, CancellationToken cancellationToken)
    {
        var completion = await _context.BlockCompletions.FirstOrDefaultAsync(
            c => c.ScheduleBlockId == request.ScheduleBlockId && c.Date == request.Date,
            cancellationToken);

        if (completion is null)
        {
            completion = new Domain.Entities.BlockCompletion
            {
                UserId = _currentUser.UserId!.Value,
                ScheduleBlockId = request.ScheduleBlockId,
                Date = request.Date,
                IsDone = request.IsDone
            };
            _context.BlockCompletions.Add(completion);
        }
        else
        {
            completion.IsDone = request.IsDone;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
