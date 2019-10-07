using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class ConfigMap : ProjectNameEntityTypeConfiguration<Config>
    {
        public override void Configure(EntityTypeBuilder<Config> builder)
        {
            base.Configure(builder);
            builder.ToTable("Config");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            //builder.Property(t => t.Value).HasColumnName("Value");
            builder.Property(t => t.VideoFile).HasColumnName("VideoFile");
            builder.Property(t => t.VideoName).HasColumnName("VideoName");
            builder.Property(t => t.Background).HasColumnName("Background");
        }
    }
}
