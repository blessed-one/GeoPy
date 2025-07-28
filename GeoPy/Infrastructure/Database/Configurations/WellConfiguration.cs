using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public class WellConfiguration : IEntityTypeConfiguration<Well>
{
    public void Configure(EntityTypeBuilder<Well> builder)
    {
        builder.HasKey(w => w.WellId);
        
        builder.Property(w => w.WellId)
            .ValueGeneratedOnAdd();
        
        builder.Property(w => w.WellNumber)
            .HasMaxLength(100);

        builder.ToTable(tb => tb.HasCheckConstraint(
            name: "ck_well_measurement_date_not_future",
            sql: "\"measurement_date\" <= CURRENT_DATE"
        ));
        
        builder.HasOne(w => w.Field)
            .WithMany(f => f.Wells)
            .HasForeignKey(w => w.FieldId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
