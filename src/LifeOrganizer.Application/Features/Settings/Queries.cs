using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Settings;

public record GetSettingsQuery : IRequest<AppSettingsDto>;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, AppSettingsDto>
{
    private readonly IApplicationDbContext _context;

    public GetSettingsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<AppSettingsDto> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var settings = await _context.AppSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return new AppSettingsDto
        {
            NotifyGym = settings?.NotifyGym ?? true,
            NotifyLanguage = settings?.NotifyLanguage ?? true,
            NotifyActivity = settings?.NotifyActivity ?? true,
            NotifyWork = settings?.NotifyWork ?? false,
            NotifySupplements = settings?.NotifySupplements ?? true,
            DefaultReminderMinutes = settings?.DefaultReminderMinutes ?? 10,
            GymLocation = settings?.GymLocation
        };
    }
}
