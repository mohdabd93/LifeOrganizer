using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Settings;

public record UpdateSettingsCommand(
    bool NotifyGym,
    bool NotifyLanguage,
    bool NotifyActivity,
    bool NotifyWork,
    bool NotifySupplements,
    int DefaultReminderMinutes,
    string? GymLocation) : IRequest;

public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateSettingsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.AppSettings.FirstOrDefaultAsync(cancellationToken);

        if (settings is null)
        {
            settings = new AppSettings { UserId = _currentUser.UserId!.Value };
            _context.AppSettings.Add(settings);
        }

        settings.NotifyGym = request.NotifyGym;
        settings.NotifyLanguage = request.NotifyLanguage;
        settings.NotifyActivity = request.NotifyActivity;
        settings.NotifyWork = request.NotifyWork;
        settings.NotifySupplements = request.NotifySupplements;
        settings.DefaultReminderMinutes = request.DefaultReminderMinutes;
        settings.GymLocation = request.GymLocation;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
