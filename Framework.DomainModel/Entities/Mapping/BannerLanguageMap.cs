using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class BannerLanguageMap : StarBerryEntityTypeWithLanguageConfiguration<BannerLanguage>
    {
        public override void Configure(EntityTypeBuilder<BannerLanguage> builder)
        {
            base.Configure(builder);
            builder.ToTable("BannerLanguage");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.Property(t => t.TimeDuration).HasColumnName("TimeDuration");
            builder.HasOne(c => c.Language).WithMany(s => s.BannerLanguages).HasForeignKey(f => f.LanguageId);
            builder.HasOne(c => c.Parent).WithMany(s => s.BannerLanguages).HasForeignKey(f => f.ParentId);
        }
    }
}