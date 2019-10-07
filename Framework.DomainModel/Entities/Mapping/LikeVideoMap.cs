using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class LikeVideoMap : StarBerryEntityTypeConfiguration<LikeVideo>
    {
        public override void Configure(EntityTypeBuilder<LikeVideo> builder)
        {
            base.Configure(builder);
            builder.ToTable("LikeVideo");
            builder.Property(t => t.Type).HasColumnName("Type");
            builder.HasOne(c => c.Customer).WithMany(s => s.LikeVideos).HasForeignKey(f => f.CustomerId);
            builder.HasOne(c => c.Video).WithMany(s => s.LikeVideos).HasForeignKey(f => f.VideoId);

        }
    }
}