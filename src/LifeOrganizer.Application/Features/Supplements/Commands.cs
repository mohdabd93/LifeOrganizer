using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Supplements;

public record CreateSupplementCommand(string Name, decimal Dose, string Unit, TimeOnly Time) : IRequest<Guid>;

public class CreateSupplementCommandValidator : AbstractValidator<CreateSupplementCommand>
{
    public CreateSupplementCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dose).GreaterThanOrEqualTo(0);
    }
}

public class CreateSupplementCommandHandler : IRequestHandler<CreateSupplementCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateSupplementCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateSupplementCommand request, CancellationToken cancellationToken)
    {
        var entity = new Supplement
        {
            UserId = _currentUser.UserId!.Value,
            Name = request.Name, Dose = request.Dose, Unit = request.Unit,
            Time = request.Time, ReminderEnabled = true
        };
        _context.Supplements.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record ToggleSupplementReminderCommand(Guid Id, bool ReminderEnabled) : IRequest;

public class ToggleSupplementReminderCommandHandler : IRequestHandler<ToggleSupplementReminderCommand>
{
    private readonly IApplicationDbContext _context;

    public ToggleSupplementReminderCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(ToggleSupplementReminderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Supplements.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (entity is null) return;

        entity.ReminderEnabled = request.ReminderEnabled;
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteSupplementCommand(Guid Id) : IRequest;

public class DeleteSupplementCommandHandler : IRequestHandler<DeleteSupplementCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteSupplementCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteSupplementCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Supplements.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.Supplements.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
