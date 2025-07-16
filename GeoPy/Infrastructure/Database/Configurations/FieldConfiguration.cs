using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.Property(f => f.FieldName)
            .HasMaxLength(100);
        
        builder.Property(f => f.AreaName)
            .HasMaxLength(100);

        builder.HasMany(f => f.Wells)
            .WithOne(w => w.Field);
    }
}