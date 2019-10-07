using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class LanguageMap : StarBerryEntityTypeConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.ToTable("Language");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.IsDefault).HasColumnName("IsDefault");
        }
    }
}