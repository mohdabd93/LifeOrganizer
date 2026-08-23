using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class MealConfiguration : IEntityTypeConfiguration<Meal>
{
    public void Configure(EntityTypeBuilder<Meal> builder)
    {
        builder.Property(m => m.Name).IsRequired().HasMaxLength(150);
        builder.HasIndex(m => m.Date);
    }
}
