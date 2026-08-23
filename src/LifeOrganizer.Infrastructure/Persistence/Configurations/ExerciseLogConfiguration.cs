using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class ExerciseLogConfiguration : IEntityTypeConfiguration<ExerciseLog>
{
    public void Configure(EntityTypeBuilder<ExerciseLog> builder)
    {
        builder.HasMany(l => l.Sets)
            .WithOne(s => s.ExerciseLog)
            .HasForeignKey(s => s.ExerciseLogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
