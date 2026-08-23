using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(e => e.Name).IsRequired().HasMaxLength(150);
        builder.Property(e => e.TargetSets).HasMaxLength(50);
        builder.Property(e => e.NextTargetWeightKg).HasPrecision(6, 2);

        builder.HasMany(e => e.Logs)
            .WithOne(l => l.Exercise)
            .HasForeignKey(l => l.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
