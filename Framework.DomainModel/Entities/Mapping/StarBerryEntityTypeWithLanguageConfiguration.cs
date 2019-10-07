using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class StarBerryEntityTypeWithLanguageConfiguration<T> : StarBerryEntityTypeConfiguration<T>
        where T : EntityWithLanguage
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.LanguageId).HasColumnName("LanguageId");
            builder.Property(c => c.ParentId).HasColumnName("ParentId");
        }
    }
}