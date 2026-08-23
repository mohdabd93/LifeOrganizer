using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.GymSplits;

public record CreateGymSplitCommand(string Name) : IRequest<Guid>;

public class CreateGymSplitCommandValidator : AbstractValidator<CreateGymSplitCommand>
{
    public CreateGymSplitCommandValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}

public class CreateGymSplitCommandHandler : IRequestHandler<CreateGymSplitCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateGymSplitCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateGymSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = new GymSplit { UserId = _currentUser.UserId!.Value, Name = request.Name };
        _context.GymSplits.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record DeleteGymSplitCommand(Guid Id) : IRequest;

public class DeleteGymSplitCommandHandler : IRequestHandler<DeleteGymSplitCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteGymSplitCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteGymSplitCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.GymSplits
            .Include(s => s.Exercises)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.Exercises.RemoveRange(entity.Exercises);
        _context.GymSplits.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
