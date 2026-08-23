using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;        

namespace LifeOrganizer.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<ScheduleBlock> ScheduleBlocks { get; }
    DbSet<BlockCompletion> BlockCompletions { get; }
    DbSet<GymSplit> GymSplits { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<ExerciseLog> ExerciseLogs { get; }
    DbSet<ExerciseSet> ExerciseSets { get; }
    DbSet<GymSession> GymSessions { get; }
    DbSet<Supplement> Supplements { get; }
    DbSet<Meal> Meals { get; }
    DbSet<LanguageWord> LanguageWords { get; }
    DbSet<LanguageProgress> LanguageProgress { get; }
    DbSet<Activity> Activities { get; }
    DbSet<AppSettings> AppSettings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
