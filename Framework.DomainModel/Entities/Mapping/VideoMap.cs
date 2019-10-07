using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class VideoMap : StarBerryEntityTypeConfiguration<Video>
    {
        public override void Configure(EntityTypeBuilder<Video> builder)
        {
            base.Configure(builder);
            builder.ToTable("Video");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.UrlLink).HasColumnName("UrlLink");
            builder.Property(t => t.ViewNumber).HasColumnName("ViewNumber");
            builder.Property(t => t.IsTrending).HasColumnName("IsTrending");
            builder.Property(t => t.IsPopular).HasColumnName("IsPopular");
            builder.Property(t => t.Avatar).HasColumnName("Avatar");
            builder.Property(t => t.Duration).HasColumnName("Duration");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.TimeDuration).HasColumnName("TimeDuration");
            builder.HasOne(c => c.Category).WithMany(s => s.Videos).HasForeignKey(f => f.CategoryId);
            builder.HasOne(s => s.Series).WithMany(v => v.Videos).HasForeignKey(f => f.SeriesId);
        }
    }
}