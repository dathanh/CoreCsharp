using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CustomerVideoWatchedMap : StarBerryEntityTypeConfiguration<CustomerVideoWatched>
    {
        public override void Configure(EntityTypeBuilder<CustomerVideoWatched> builder)
        {
            base.Configure(builder);
            builder.ToTable("CustomerVideoWatched");
            builder.Property(t => t.VideoId).HasColumnName("VideoId");
            builder.Property(t => t.CustomerId).HasColumnName("CustomerId");
            builder.Property(t => t.Status).HasColumnName("Status");
            builder.Property(t => t.CurrentDuration).HasColumnName("CurrentDuration");
            builder.HasOne(c => c.Video).WithMany(s => s.CustomerVideoWatcheds).HasForeignKey(f => f.VideoId);
            builder.HasOne(c => c.Customer).WithMany(s => s.CustomerVideoWatcheds).HasForeignKey(f => f.CustomerId);
        }
    }
}