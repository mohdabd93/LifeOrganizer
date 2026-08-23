using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class ScheduleBlockConfiguration : IEntityTypeConfiguration<ScheduleBlock>
{
    public void Configure(EntityTypeBuilder<ScheduleBlock> builder)
    {
        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.Property(b => b.ColorHex).IsRequired().HasMaxLength(9);

        builder.HasMany(b => b.Completions)
            .WithOne(c => c.ScheduleBlock)
            .HasForeignKey(c => c.ScheduleBlockId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
