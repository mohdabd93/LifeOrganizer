using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class GymSplitConfiguration : IEntityTypeConfiguration<GymSplit>
{
    public void Configure(EntityTypeBuilder<GymSplit> builder)
    {
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);

        builder.HasMany(s => s.Exercises)
            .WithOne(e => e.GymSplit)
            .HasForeignKey(e => e.GymSplitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
