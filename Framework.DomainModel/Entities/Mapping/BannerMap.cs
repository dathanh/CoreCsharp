using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class BannerMap : StarBerryEntityTypeConfiguration<Banner>
    {
        public override void Configure(EntityTypeBuilder<Banner> builder)
        {
            base.Configure(builder);
            builder.ToTable("Banner");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.Property(t => t.UrlLink).HasColumnName("UrlLink");
            builder.Property(t => t.OrderNumber).HasColumnName("OrderNumber");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.Type).HasColumnName("Type");
            builder.Property(t => t.TimeDuration).HasColumnName("TimeDuration");
            builder.Property(t => t.VideoFile).HasColumnName("VideoFile");
            builder.Property(t => t.VideoOriginName).HasColumnName("VideoOriginName");
            builder.Property(t => t.IsHideDescription).HasColumnName("IsHideDescription");
            builder.HasOne(s => s.Video).WithMany(v => v.Banners).HasForeignKey(f => f.VideoId);
        }
    }
}