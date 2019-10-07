using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class VideoPlayListMap : StarBerryEntityTypeConfiguration<VideoPlayList>
    {
        public override void Configure(EntityTypeBuilder<VideoPlayList> builder)
        {
            base.Configure(builder);
            builder.ToTable("VideoPlayList");
            builder.Property(t => t.PlayListId).HasColumnName("PlayListId");
            builder.Property(t => t.VideoId).HasColumnName("VideoId");
            builder.Property(t => t.BannerId).HasColumnName("BannerId");
            builder.HasOne(p => p.PlayList).WithMany(v => v.VideoPlayLists).HasForeignKey(f => f.PlayListId);
            builder.HasOne(s => s.Video).WithMany(v => v.VideoPlayLists).HasForeignKey(f => f.VideoId);
            builder.HasOne(s => s.Banner).WithMany(v => v.VideoPlayLists).HasForeignKey(f => f.BannerId);
        }
    }

}