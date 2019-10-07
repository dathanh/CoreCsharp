using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.DomainModel.Entities.Mapping
{
    public class CommentMap : StarBerryEntityTypeConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);
            builder.ToTable("Comment");
            builder.Property(t => t.Message).HasColumnName("Message");
            builder.Property(t => t.IsActive).HasColumnName("IsActive");
            builder.HasOne(c => c.Customer).WithMany(s => s.Comments).HasForeignKey(f => f.CustomerId);
            builder.HasOne(s => s.Video).WithMany(v => v.Comments).HasForeignKey(f => f.VideoId);
            builder.HasOne(s => s.Parent).WithMany(v => v.Children).HasForeignKey(f => f.ParentId);
        }
    }
}