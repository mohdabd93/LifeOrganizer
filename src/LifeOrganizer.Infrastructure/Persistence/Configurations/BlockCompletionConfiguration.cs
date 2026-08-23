using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class BlockCompletionConfiguration : IEntityTypeConfiguration<BlockCompletion>
{
    public void Configure(EntityTypeBuilder<BlockCompletion> builder)
    {
        builder.HasIndex(c => new { c.ScheduleBlockId, c.Date }).IsUnique();
    }
}
