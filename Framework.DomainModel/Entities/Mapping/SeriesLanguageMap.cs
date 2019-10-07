using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class SeriesLanguageMap : StarBerryEntityTypeWithLanguageConfiguration<SeriesLanguage>
    {
        public override void Configure(EntityTypeBuilder<SeriesLanguage> builder)
        {
            base.Configure(builder);
            builder.ToTable("SeriesLanguage");
            builder.Property(s => s.Name).HasColumnName("Name");
            builder.Property(s => s.Description).HasColumnName("Description");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.HasOne(s => s.Parent).WithMany(c => c.SeriesLanguages).HasForeignKey(f => f.ParentId);
            builder.HasOne(s => s.Language).WithMany(c => c.SeriesLanguages).HasForeignKey(f => f.LanguageId);
        }
    }
}
