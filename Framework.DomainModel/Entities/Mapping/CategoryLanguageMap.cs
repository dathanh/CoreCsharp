using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CategoryLanguageMap : StarBerryEntityTypeWithLanguageConfiguration<CategoryLanguage>
    {
        public override void Configure(EntityTypeBuilder<CategoryLanguage> builder)
        {
            base.Configure(builder);
            builder.ToTable("CategoryLanguage");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.HasOne(c => c.Language).WithMany(s => s.CategoryLanguages).HasForeignKey(f => f.LanguageId);
            builder.HasOne(c => c.Parent).WithMany(s => s.CategoryLanguages).HasForeignKey(f => f.ParentId);
        }
    }
}