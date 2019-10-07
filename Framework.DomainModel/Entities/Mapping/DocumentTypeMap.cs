using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class DocumentTypeMap : StarBerryEntityTypeConfiguration<DocumentType>
    {
        public override void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            base.Configure(builder);
            builder.Property(t => t.Id).ValueGeneratedNever();
            builder.ToTable("DocumentType");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Title).HasColumnName("Title");
            builder.Property(t => t.Order).HasColumnName("Order");
        }
    }
}