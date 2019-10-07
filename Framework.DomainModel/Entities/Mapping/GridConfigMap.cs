using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class GridConfigMap : StarBerryEntityTypeConfiguration<GridConfig>
    {
        public override void Configure(EntityTypeBuilder<GridConfig> builder)
        {
            base.Configure(builder);

            builder.ToTable("GridConfig");
            builder.Property(t => t.DocumentTypeId).HasColumnName("DocumentTypeId");
            builder.Property(t => t.UserId).HasColumnName("UserId");
            builder.Property(t => t.XmlText).HasColumnName("XmlText");
            builder.Property(t => t.GridInternalName).HasColumnName("GridInternalName");

            builder.HasOne(t => t.DocumentType)
                .WithMany(t => t.GridConfigs)
                .HasForeignKey(d => d.DocumentTypeId);

            builder.HasOne(t => t.User)
                .WithMany(t => t.GridConfigs)
                .HasForeignKey(d => d.UserId);
        }
    }
}
