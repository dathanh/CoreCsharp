using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class PlayListMap : StarBerryEntityTypeConfiguration<PlayList>
    {
        public override void Configure(EntityTypeBuilder<PlayList> builder)
        {
            base.Configure(builder);
            builder.ToTable("PlayList");
            builder.Property(s => s.Name).HasColumnName("Name");
            builder.Property(s => s.Description).HasColumnName("Description");
            builder.Property(s => s.OrderNumber).HasColumnName("OrderNumber");
            builder.Property(s => s.IsActive).HasColumnName("IsActive");
            builder.Property(s => s.Background).HasColumnName("Background");
            builder.HasOne(c => c.Customer).WithMany(s => s.PlayLists).HasForeignKey(f => f.OwnerCustomerId);

        }
    }
}
