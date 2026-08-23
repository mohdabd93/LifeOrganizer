using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class LanguageProgressConfiguration : IEntityTypeConfiguration<LanguageProgress>
{
    public void Configure(EntityTypeBuilder<LanguageProgress> builder)
    {
        builder.Property(p => p.CurrentLevel).IsRequired().HasMaxLength(10);
        builder.Property(p => p.TargetLevel).IsRequired().HasMaxLength(10);
    }
}
