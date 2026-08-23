using LifeOrganizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Api.Persistence;

 public static class DbInitializer
{
    public static async Task MigrateAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}
