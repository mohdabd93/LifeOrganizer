using LifeOrganizer.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LifeOrganizer.Infrastructure.Persistence;
 
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private class DesignTimeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId => null;
    }

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=DESKTOP-LJ2VGBM\\SQLEXPRESS;Database=LifeOrganizerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

        return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeCurrentUserService());
    }
}
