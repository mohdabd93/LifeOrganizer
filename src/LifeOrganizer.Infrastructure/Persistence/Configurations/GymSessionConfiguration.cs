using LifeOrganizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeOrganizer.Infrastructure.Persistence.Configurations;

public class GymSessionConfiguration : IEntityTypeConfiguration<GymSession>
{
    public void Configure(EntityTypeBuilder<GymSession> builder)
    {
        builder.HasIndex(s => s.Date);
    }
}
