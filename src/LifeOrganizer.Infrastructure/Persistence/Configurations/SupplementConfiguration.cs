using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class SupplementConfiguration : IEntityTypeConfiguration<Supplement>
{
    public void Configure(EntityTypeBuilder<Supplement> builder)
    {
        builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Unit).IsRequired().HasMaxLength(20);
        builder.Property(s => s.Dose).HasPrecision(6, 2);
    }
}
