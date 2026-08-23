using LifeOrganizer.Application.Common.Interfaces;

namespace LifeOrganizer.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);
    public DateTime Now => DateTime.Now;
}
