using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class VideoLanguageMap : StarBerryEntityTypeWithLanguageConfiguration<VideoLanguage>
    {
        public override void Configure(EntityTypeBuilder<VideoLanguage> builder)
        {
            base.Configure(builder);
            builder.ToTable("VideoLanguage");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.TimeDuration).HasColumnName("TimeDuration");
            builder.Property(t => t.Avatar).HasColumnName("Avatar");
            builder.HasOne(c => c.Language).WithMany(s => s.VideoLanguages).HasForeignKey(f => f.LanguageId);
            builder.HasOne(c => c.Parent).WithMany(s => s.VideoLanguages).HasForeignKey(f => f.ParentId);
        }
    }
}