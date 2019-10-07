using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class PlayListLanguageMap : StarBerryEntityTypeWithLanguageConfiguration<PlayListLanguage>
    {
        public override void Configure(EntityTypeBuilder<PlayListLanguage> builder)
        {
            base.Configure(builder);
            builder.ToTable("PlayListLanguage");
            builder.Property(s => s.Name).HasColumnName("Name");
            builder.Property(s => s.Description).HasColumnName("Description");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.HasOne(s => s.Parent).WithMany(c => c.PlayListLanguages).HasForeignKey(f => f.ParentId);
            builder.HasOne(s => s.Language).WithMany(c => c.PlayListLanguages).HasForeignKey(f => f.LanguageId);
        }
    }
}
