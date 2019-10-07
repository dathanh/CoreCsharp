using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CategoryMap : StarBerryEntityTypeConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.ToTable("Category");
            builder.Property(t => t.Name).HasColumnName("Name");
            builder.Property(t => t.Description).HasColumnName("Description");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.Property(t => t.ParentId).HasColumnName("ParentId");
            //builder.Property(t => t.Avatar).HasColumnName("Avatar");
            builder.Property(t => t.Background).HasColumnName("Background");
            builder.HasOne(c => c.Parent).WithMany(s => s.Children).HasForeignKey(f => f.ParentId);
        }
    }
}