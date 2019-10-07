using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SeriesMap : StarBerryEntityTypeConfiguration<Series>
    {
        public override void Configure(EntityTypeBuilder<Series> builder)
        {
            base.Configure(builder);
            builder.ToTable("Series");
            builder.Property(s => s.Name).HasColumnName("Name");
            builder.Property(s => s.Description).HasColumnName("Description");
            builder.Property(s => s.Background).HasColumnName("Background");
            builder.Property(s => s.Status).HasColumnName("Status");
            builder.Property(s => s.OrderNumber).HasColumnName("OrderNumber");
            builder.Property(s => s.IsActive).HasColumnName("IsActive");
        }
    }
}
