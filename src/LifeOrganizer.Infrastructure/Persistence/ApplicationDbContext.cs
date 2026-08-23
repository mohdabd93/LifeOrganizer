using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{ 
    private readonly Guid? _currentUserId;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserId = currentUserService.UserId;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<ScheduleBlock> ScheduleBlocks => Set<ScheduleBlock>();
    public DbSet<BlockCompletion> BlockCompletions => Set<BlockCompletion>();
    public DbSet<GymSplit> GymSplits => Set<GymSplit>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<ExerciseSet> ExerciseSets => Set<ExerciseSet>();
    public DbSet<GymSession> GymSessions => Set<GymSession>();
    public DbSet<Supplement> Supplements => Set<Supplement>();
    public DbSet<Meal> Meals => Set<Meal>();
    public DbSet<LanguageWord> LanguageWords => Set<LanguageWord>();
    public DbSet<LanguageProgress> LanguageProgress => Set<LanguageProgress>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
         
        builder.Entity<ScheduleBlock>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<BlockCompletion>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<GymSplit>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<Exercise>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<ExerciseLog>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<ExerciseSet>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<GymSession>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<Supplement>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<Meal>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<LanguageWord>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<LanguageProgress>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<Activity>().HasQueryFilter(x => x.UserId == _currentUserId);
        builder.Entity<AppSettings>().HasQueryFilter(x => x.UserId == _currentUserId);

        base.OnModelCreating(builder);
    }
}
