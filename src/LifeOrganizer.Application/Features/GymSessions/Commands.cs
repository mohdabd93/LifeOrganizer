using LifeOrganizer.Application.Common.Exceptions;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.GymSessions;

public record StartGymSessionCommand : IRequest<Guid>;

public class StartGymSessionCommandHandler : IRequestHandler<StartGymSessionCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ICurrentUserService _currentUser;

    public StartGymSessionCommandHandler(IApplicationDbContext context, IDateTime dateTime, ICurrentUserService currentUser)
    {
        _context = context;
        _dateTime = dateTime;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(StartGymSessionCommand request, CancellationToken cancellationToken)
    {
        var now = _dateTime.Now;
        var entity = new GymSession
        {
            UserId = _currentUser.UserId!.Value,
            Date = DateOnly.FromDateTime(now),
            StartTime = TimeOnly.FromDateTime(now)
        };

        _context.GymSessions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record EndGymSessionCommand(Guid Id) : IRequest;

public class EndGymSessionCommandHandler : IRequestHandler<EndGymSessionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public EndGymSessionCommandHandler(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task Handle(EndGymSessionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.GymSessions.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(GymSession), request.Id);

        var now = _dateTime.Now;
        var endTime = TimeOnly.FromDateTime(now);
        entity.EndTime = endTime;

        var start = entity.Date.ToDateTime(entity.StartTime);
        var end = entity.Date.ToDateTime(endTime);
        entity.DurationMinutes = Math.Max(1, (int)Math.Round((end - start).TotalMinutes));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
