using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class WellConfiguration : IEntityTypeConfiguration<Well>
{
    public void Configure(EntityTypeBuilder<Well> builder)
    {
        builder.Property(w => w.WellNumber)
            .HasMaxLength(100);

        builder.HasOne(w => w.Field)
            .WithMany(f => f.Wells);
    }
}