using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class LanguageWordConfiguration : IEntityTypeConfiguration<LanguageWord>
{
    public void Configure(EntityTypeBuilder<LanguageWord> builder)
    {
        builder.Property(w => w.TargetLanguageText).IsRequired().HasMaxLength(150);
        builder.Property(w => w.TranslationText).IsRequired().HasMaxLength(150);
    }
}
